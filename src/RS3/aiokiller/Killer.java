package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.*;
import org.powerbot.script.rt6.*;
import org.powerbot.script.rt6.ClientContext;

import java.util.concurrent.Callable;

public class Killer extends RS3Task {
    private final static int CHICKENS[] = {1017, 41};


    public Killer(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {

//
//        if(AIOKiller.looting)
//            return false;
//
//        AIOKiller.looting = ctx.npcs.size() <= 4;

        return ctx.backpack.select().count() < 28/* && (!ctx.players.local().inCombat() || ctx.players.local().animation() == -1)*/;
    }

    @Override
    public void execute() {
        System.out.println("Killer.execute");

        final Npc chicken = ctx.npcs.select().id(CHICKENS).select(new Filter<Npc>() {
            @Override
            public boolean accept(Npc npc) {
                System.out.println(!npc.inCombat());
                System.out.println(npc.healthPercent() > 0);
                System.out.println(!npc.name().equals("*Chicken"));
                System.out.println(npc.animation() != 3806);
                System.out.println(npc.tile().y() <= 3287);
                System.out.println(npc.tile().x() <= 3210);
                System.out.println(npc.tile().x() >= 3204);
                return !npc.inCombat() &&
                        npc.healthPercent() > 0 &&
                        !npc.name().equals("*Chicken") &&
                        npc.animation() != 3806 &&
                        npc.tile().y() <= 3287 &&
                        npc.tile().x() <= 3210 &&
                        npc.tile().x() >= 3204;
            }
        }).nearest().poll();

        System.out.println("Anim: " + chicken.animation());

        if(!chicken.tile().matrix(ctx).reachable()){
            System.out.println("Chicken not reachable");
            AIOKiller.gateClosed = true;
            return;
        }

        BringToViewPort(chicken);
        chicken.interact("Attack");

        Condition.sleep(Random.nextInt(50,300));


//        Condition.wait(new Callable<Boolean>() {
//            @Override
//            public Boolean call() throws Exception {
//                return ctx.players.local().inCombat() && chicken.valid();
//            }
//        },50, 5);
    }

    private void BringToViewPort(final Npc obj){
        if(!obj.inViewport()){
            System.out.println("Bring to Viewport");
            ctx.camera.turnTo(obj);
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return obj.inViewport();
                }
            }, 150, 5);
        }
    }
}
