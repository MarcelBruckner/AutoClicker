package RS3;

import com.sun.istack.internal.Nullable;
import org.powerbot.script.*;
import org.powerbot.script.rt6.ChatOption;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Npc;
import org.powerbot.script.rt6.Objects;

import java.util.concurrent.Callable;

@Script.Manifest(name="Tutorial", description="Runs through tutorial island", properties = "author=Marcel; topic=999; client=6;")
public class Tutorial extends PollingScript<ClientContext>{

    final static String INTERACTIONS[] = {"Talk to", "Follow", "Attack", "Lure"};

    final static int GUDRIK[] = {18589, 18590};
    final static int ENEMIES[] = {18594, 18595 };
    final static int SPOTS[] = {19297 };

    @Override
    public void start(){
        System.out.println("Started");
    }

    @Override
    public void stop(){
        System.out.println("Stopped");
    }

    @Override
    public void poll() {
        final Npc enemy = ctx.npcs.select().id(ENEMIES).nearest().poll();
        final Npc spot = ctx.npcs.select().id(SPOTS).nearest().poll();

        if(spot.valid() && ctx.backpack.items().length < 3)
            Fishing(spot);
        else if(enemy.valid())
            FightEnemy(enemy);
        else if(!ctx.chat.chatting())
            WalkToGudrik();
        else if(ctx.chat.chatting()){
            TalkToGudrik();
        }
    }

    public void Fishing(Npc spot){
        spot.interact(INTERACTIONS[3]);

        BringToViewport(spot);

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return null;
            }
        }, 150, 20);
    }

    public void FightEnemy(final Npc enemy){
        if(enemy.inCombat())
            return;

        BringToViewport(enemy);

        enemy.interact(INTERACTIONS[2]);

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return enemy.inCombat();
            }
        }, 150, 20);
    }

    public void TalkToGudrik() {

        if (ctx.chat.canContinue()) {
            ctx.chat.clickContinue(true);
        } else {
            ChatOption chat = ctx.chat.select().text("Let's go").poll();
            chat.select(true);
        }
        Condition.sleep(Random.nextInt(350, 500));

    }

    public void WalkToGudrik(){
        final Npc gudrik = ctx.npcs.select().id(GUDRIK).nearest().poll();

        BringToViewport(gudrik);

        for (String s : INTERACTIONS) {
            gudrik.interact(s);
        }

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return ctx.chat.chatting();
            }
        }, 150, 20);
    }

    public void BringToViewport(Npc view){
        final Npc v = view;

        if(!v.inViewport()) {
            ctx.camera.turnTo(v);

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return v.inViewport();
                }
            }, 150, 20);
        }
    }
}
