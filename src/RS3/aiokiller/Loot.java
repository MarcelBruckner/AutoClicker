package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Filter;
import org.powerbot.script.Random;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.*;

import java.util.concurrent.Callable;

public class Loot extends RS3Task {
    final int ITEMS[] = {526,2183,314};
    GroundItem loot;

    public Loot(ClientContext ctx) {
        super(ctx);

    }

    @Override
    public boolean activate() {
//
        int b = 33000 - ctx.backpack.items().length * 1000;
        if(System.currentTimeMillis() - AIOKiller.lastLoot < Random.nextInt(b - 5000, b+5000))
            return false;

        for (int i = 0; i < 5; i++) {
            loot = ctx.groundItems.select(new Filter<GroundItem>() {
                @Override
                public boolean accept(GroundItem groundItem) {
                    return !groundItem.tile().equals(new Tile(3204, 3287, 0)) &&
                            groundItem.tile().distanceTo(ctx.players.local().tile()) < 10 &&
                            groundItem.tile().y() <= 3287 &&
                            groundItem.tile().x() <= 3210 &&
                            groundItem.tile().x() >= 3204;
                }
            }).id(ITEMS).nearest().poll();

            if(loot != null && ctx.backpack.select().count() <= 27 && loot.valid()){
                return true;
            }
        }

        return false;
        //        System.out.println("Size: " + ctx.groundItems.size());
//        System.out.println("Count: " + ctx.groundItems.count());
//
//
//        if(ctx.groundItems.isEmpty()){
//            System.out.println("No loot");
//            AIOKiller.looting = false;
//            return false;
//        }
//
//        loot = ctx.groundItems.nearest().poll();
//        if(!loot.tile().matrix(ctx).reachable()){
//            System.out.println("Loot not reachable");
//            AIOKiller.gateClosed = true;
//            return false;
//        }
       /* System.out.println("Loot != null: " + (loot != null));
        System.out.println("ctx.backpack.select().count() <= 27: " + (ctx.backpack.select().count() <= 27));
*/
//        System.out.println("Loot valid: " + loot.valid());

//        if(ctx.groundItems.isEmpty())
//            lastLoot = System.currentTimeMillis();

//        System.out.println(System.currentTimeMillis() - lastLoot);

//        return /*System.currentTimeMillis() - lastLoot > 10000 &&*/ loot != null && ctx.backpack.select().count() <= 27 && loot.valid();
    }

    @Override
    public void execute() {
        System.out.println("Loot.execute");

        if(!loot.tile().matrix(ctx).reachable()){
            System.out.println("Loot not reachable");
            AIOKiller.gateClosed = true;
            return;
        }

        if (loot.inViewport() || Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                ctx.camera.turnTo(loot);
                return loot.inViewport();
            }
        }, 150, 5)) {
            if (loot.click() /*loot.interact("Take")*/) {
                Condition.wait(new Callable<Boolean>() {
                    @Override
                    public Boolean call() throws Exception {
                        return ctx.widgets.component(1622, 24).visible();
                    }
                }, 150, 5);
            }

        }
    }
}