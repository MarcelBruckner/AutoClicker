package RS3;

import RS3.aiokiller.*;
import com.sun.corba.se.impl.orbutil.closure.Constant;
import com.sun.xml.internal.bind.v2.runtime.reflect.opt.Const;
import org.powerbot.script.PaintListener;
import org.powerbot.script.PollingScript;
import org.powerbot.script.Script;
import org.powerbot.script.rt6.ClientAccessor;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Constants;
import org.powerbot.script.rt6.Skills;
import z.Con;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.List;

@Script.Manifest(name = "AIOKiller", description = "Kills anything", properties = "author=Marcel, topic=Combat, client=6")

public class AIOKiller extends PollingScript<ClientContext> implements PaintListener {

    List<RS3Task> taskList = new ArrayList<RS3Task>();

    int startXp;
    public static int backpackValue = 0;
    public static int overallValue = 0;

    public static long lastLoot = 0;
    public static int runs = 0;

    public static boolean gateClosed = false;

    final int SKILLS[] = {Constants.SKILLS_CONSTITUTION, Constants.SKILLS_ATTACK, Constants.SKILLS_STRENGTH, Constants.SKILLS_DEFENSE, Constants.SKILLS_MAGIC, Constants.SKILLS_RANGE, Constants.SKILLS_PRAYER};

    XPCounter xpCounter;

    @Override
    public void start() {
//        String userOptions[] = {"Bank", "Bury"};
//        String userChoice = "" + (String)JOptionPane.showInputDialog(null, "Bank or bury bones.", "Chicken Killer", JOptionPane.PLAIN_MESSAGE, null, userOptions, userOptions[0]);
//
//        if(userChoice.equals("Bury")){
//            System.out.println("Bury");
//        }else if(userChoice.equals("Bank")){
//            System.out.println("Bank");
//        }else{
//            ctx.controller.stop();
//        }

        xpCounter = new XPCounter(ctx, SKILLS);

     //   taskList.add(new CountBackpack(ctx));
        taskList.add(new LootAll(ctx));
        taskList.add(new OpenGate(ctx));
        taskList.add(new WalkToChickens(ctx));
        taskList.add(new Loot(ctx));
        taskList.add(new Killer(ctx));
        taskList.add(new Bank(ctx));
        taskList.add(new WalkToBank(ctx));
    }

    @Override
    public void poll() {
        for (RS3Task task : taskList) {
            if (task.activate()) {
                task.execute();
                break;
            }
        }
    }

    @Override
    public void repaint(Graphics graphics) {
        long ms = this.getTotalRuntime();
        long sec = (ms / 1000) % 60;
        long min = (ms / (1000 * 60)) % 60;
        long hr = (ms / (1000 * 60 * 60)) % 24;

        Graphics2D g = (Graphics2D) graphics;

        g.setColor(new Color(0,0,0,180));
        g.fillRect(10,10,150,100);

        g.setColor(new Color(255,255,255));
        g.drawRect(10,10,150,100);

        g.drawString("Chicken Killer", 20,25);
        g.drawString("Running: " + String.format("%02d:%02d:%02d", hr,min,sec), 20, 40);
        g.drawString("Exp/Hour: " + (int)(xpCounter.gainedXP() * (3600000D / ms)), 20, 55);
        g.drawString("Overall XP: " + xpCounter.gainedXP(), 20, 70);
//        g.drawString("Gained GP: " + overallValue, 20, 85);
        g.drawString("Runs: " + runs, 20, 85);
    }
}
