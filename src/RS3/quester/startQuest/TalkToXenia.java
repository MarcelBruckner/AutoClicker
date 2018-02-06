package RS3.quester.startQuest;

import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ChatOption;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Npc;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;

public class TalkToXenia extends StartQuestTask {

    final List<String> chatOptions = new ArrayList<String>() {{
        add("What help do you need?");
        add("I'll help you.");
    }};

    public TalkToXenia(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        System.out.println("Valid: " + ctx.widgets.component(acceptButton[0], acceptButton[1]).visible());
        return ctx.players.local().tile().distanceTo(xeniaLoc) < 3 &&
                !ctx.widgets.component(acceptButton[0], acceptButton[1]).visible();
    }

    @Override
    public void execute() {
        final Npc x = ctx.npcs.select().id(xenia).nearest().poll();
        if (!x.inViewport()) {
            ctx.camera.turnTo(x);
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return x.inViewport();
                }
            }, 150, 5);
        } else if (!ctx.chat.chatting() && !ctx.chat.canContinue()) {
            System.out.println("interacting");
            x.interact("Talk to");
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return ctx.chat.chatting();
                }
            }, 150, 20);
        } else {
            if (ctx.chat.canContinue()) {
                System.out.println("Can continue");
                ctx.chat.clickContinue(true);
            } else {
                System.out.println("Choosing options");
                List<ChatOption> options = ctx.chat.get();
                for (ChatOption o : options) {
                    System.out.println(o.text());
                    if (chatOptions.contains(o.text())) {
                        o.select(true);
                        Condition.sleep(Random.nextInt(1000,1500));
                        break;
                    }
                }
            }
            Condition.sleep(Random.nextInt(100, 200));
        }
    }
}
