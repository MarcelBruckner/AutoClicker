package RS3;

import com.sun.istack.internal.Nullable;
import org.powerbot.script.*;
import org.powerbot.script.rt6.*;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Interactive;

import java.util.Locale;
import java.util.concurrent.Callable;

@Script.Manifest(name = "Tutorial", description = "Runs through tutorial island", properties = "author=Marcel; topic=999; client=6;")
public class Tutorial extends PollingScript<ClientContext> {

    final static String INTERACTIONS[] = {"Talk to", "Follow", "KillRanger", "Lure", "Use", "Inspect", "Take", "Mine"};

    final static int NPCS[] = {18594, 18595, 18596, 18589, 18590};
    final static int ENEMIES[] = {18594, 18595, 18597};
    final static int SPOTS[] = {19297, 88876};
    final static int BACKPACK_ITEMS[] = {30076};
    final static int OBJECTS[] = {88246};
    final static int GROUND_ITEMS[] = {30069};

    @Override
    public void start() {
        System.out.println("Started");
    }

    @Override
    public void stop() {
        System.out.println("Stopped");
    }

    @Override
    public void poll() {
        final Npc enemy = ctx.npcs.select().id(ENEMIES).nearest().poll();
        final Npc spot = ctx.npcs.select().id(SPOTS).nearest().poll();
        final GameObject fire = ctx.objects.select().id(OBJECTS).nearest().poll();
        final GroundItem coins = ctx.groundItems.select().id(GROUND_ITEMS).nearest().poll();

        Component component = ctx.widgets.component(669, 18);
        if (component.visible())


            if (coins.valid())
                PickUp(coins);
            else if (fire.valid() && ctx.backpack.select().id(BACKPACK_ITEMS).count() > 0)
                Cook(fire);
            else if (spot.valid() && ctx.backpack.select().id(BACKPACK_ITEMS).count() < 3)
                Fishing(spot);
            else if (enemy.valid())
                FightEnemy(enemy);
            else
                Interact();
//        else if(!ctx.chat.chatting())
//            WalkToGudrik();
//        else if(ctx.chat.chatting()){
//            TalkToGudrik();
//        }
    }

    public void Mine(GameObject rock) {
        BringToViewport(rock);

        rock.interact(INTERACTIONS[7]);

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !ctx.objects.select().id(GROUND_ITEMS).nearest().poll().valid();
            }
        }, 150, 20);
    }

    public void PickUp(GroundItem item) {
        BringToViewport(item);

        item.interact(INTERACTIONS[6]);

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !ctx.groundItems.select().id(GROUND_ITEMS).nearest().poll().valid();
            }
        }, 150, 20);
    }

    public void Cook(GameObject fire) {

        Component component = ctx.widgets.component(1370, 38);
        if (component.visible()) {
            component.click();
            Sleep();
        } else {
            BringToViewport(fire);

            Item fish = ctx.backpack.select().id(BACKPACK_ITEMS).poll();
            fish.interact(INTERACTIONS[4]);
            fire.interact(INTERACTIONS[4]);

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return ctx.backpack.select().id(BACKPACK_ITEMS).count() == 0;
                }
            }, 150, 20);
        }
    }

    public void Fishing(Npc spot) {
        BringToViewport(spot);

        spot.interact(INTERACTIONS[3]);

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return null;
            }
        }, 150, 20);
    }

    public void FightEnemy(final Npc enemy) {
        if (enemy.inCombat())
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

    public void Interact() {
        if (ctx.chat.chatting()) {
            if (ctx.chat.canContinue()) {
                ctx.chat.clickContinue(true);
            } else {
                ChatOption chat = ctx.chat.select().text("Let's go").poll();
                chat.select(true);
            }
            Sleep();

        } else {
            Npc npc = new Npc(ctx, null);
            for (int i : NPCS) {
                npc = ctx.npcs.select().id(i).nearest().poll();
                if (npc.valid())
                    break;
            }

            System.out.println(npc);

            BringToViewport(npc);

            for (String s : INTERACTIONS) {
                npc.interact(s);
            }

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return ctx.chat.chatting();
                }
            }, 150, 20);
        }
    }

//    public void TalkToGudrik() {
//
//        if (ctx.chat.canContinue()) {
//            ctx.chat.clickContinue(true);
//        } else {
//            ChatOption chat = ctx.chat.select().text("Let's go").poll();
//            chat.select(true);
//        }
//
//        Sleep();
//    }
//
//    public void WalkToGudrik(){
//        final Npc gudrik = ctx.npcs.select().id(GUDRIK).nearest().poll();
//
//        BringToViewport(gudrik);
//
//        for (String s : INTERACTIONS) {
//            gudrik.interact(s);
//        }
//
//        Condition.wait(new Callable<Boolean>() {
//            @Override
//            public Boolean call() throws Exception {
//                return ctx.chat.chatting();
//            }
//        }, 150, 20);
//    }

    public <T> void BringToViewport(T view) {

        final Interactive v;

        if (view instanceof GameObject)
            v = (GameObject) view;
        else if (view instanceof Npc)
            v = (Npc) view;
        else if (view instanceof GroundItem)
            v = (GroundItem) view;
        else
            return;

        if (!v.inViewport()) {
            if (view instanceof GameObject)
                ctx.camera.turnTo((GameObject) v);
            else if (view instanceof Npc)
                ctx.camera.turnTo((Npc) v);
            else if (view instanceof GroundItem)
                ctx.camera.turnTo((GroundItem) v);
            else
                return;

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return v.inViewport();
                }
            }, 150, 20);
        }
    }

    public void Sleep() {
        Condition.sleep(Random.nextInt(350, 500));
    }
}
