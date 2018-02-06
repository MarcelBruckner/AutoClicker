package RS3;

import RS3.smither.Bank_Smither;
import RS3.smither.Smelt;
import org.powerbot.script.PaintListener;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.PollingScript;
import org.powerbot.script.Script;
import org.powerbot.script.rt6.Constants;

import java.awt.*;
import java.util.ArrayList;
import java.util.List;

@Script.Manifest(name = "Smither", description = "Smiths", properties = "client=6; author=Marcel; topic=999")


public class Smither extends PollingScript<ClientContext> implements PaintListener{
    List<RS3Task> tasks = new ArrayList<RS3Task>();
    XPCounter xpCounter = new XPCounter(ctx, new int[]{Constants.SKILLS_SMITHING});;

    int lastXP = xpCounter.gainedXP();
    int smeltedBars = 0;

    int withdrawn[] = {2349};
    int amount[] = {28};
    int processor = 12692;
    int makeable[] = {1117, 1075};
    int makeIfMoreThan[] = {5,3};

    public void start(){
        tasks.add(new Smelt(ctx, false, withdrawn,processor,makeable,makeIfMoreThan));
        tasks.add(new Bank_Smither(ctx, withdrawn, amount));
    }

    @Override
    public void poll() {
        for (RS3Task task : tasks) {
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
        g.fillRect(10,10,250,110);

        g.setColor(new Color(255,255,255));
        g.drawRect(10,10,250,110);

        g.drawString("Lumbridge Smelter", 20,25);
        g.drawString("Running: " + String.format("%02d:%02d:%02d", hr,min,sec), 20, 40);

        int expH = (int)(xpCounter.gainedXP() * (3600000D / ms));
        g.drawString("Exp/Hour: " + expH, 20, 55);
        g.drawString("Overall XP: " + xpCounter.gainedXP(), 20, 70);
        if(lastXP != xpCounter.gainedXP()){
            smeltedBars++;
            lastXP = xpCounter.gainedXP();
        }
        g.drawString("Bars smelted: " + smeltedBars, 20, 85);
        g.drawString("Remaining XP: " + xpCounter.remainingXP()[0], 20, 100);

        float rhr;
        if(expH != 0) {
            rhr = (xpCounter.remainingXP()[0]) / (expH * 1.0f) ;
            min = (long) ((rhr * 60)  % 60);
            sec = (long) ((rhr * 3600) % 60);
        }else{
            rhr = min = sec = 0;
        }
        g.drawString("Time till level up: " + String.format("%02d:%02d:%02d", (int)rhr,min,sec), 20, 115);
    }
}
