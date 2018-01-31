package RS3.quester.startQuest;

import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ChatOption;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Npc;

import java.util.ArrayList;
import java.util.List;

public class TalkToXenia extends StartQuestTask {

    final List<String> chatOptions = new ArrayList<String>(){{
        add("What help do you need?");
        add("I'll help you.");
    }};

    public TalkToXenia(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.players.local().tile().distanceTo(xeniaLoc) < 3 &&
                !acceptButton.visible();
    }

    @Override
    public void execute() {
        Npc x = ctx.npcs.select().id(xenia).nearest().poll();
        if(!x.inViewport()){
            ctx.camera.turnTo(x);
        }else if(!ctx.chat.chatting()){
            x.interact("Talk to");
        }else if(ctx.chat.canContinue()){
            ctx.chat.clickContinue(true);
        }else{
            List<ChatOption> options = ctx.chat.get();
            System.out.println("TalkToXenia.execute");
            for(ChatOption o : options) {
                System.out.println(o.text());
                if(chatOptions.contains(o.text())){
                    o.select(true);
                    break;
                }
            }
            Condition.sleep(Random.nextInt(100,200));
        }
    }
}
