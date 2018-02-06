package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Component;


public class LootAll extends RS3Task {

    Component takeAll;

    public LootAll(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        takeAll = ctx.widgets.component(1622, 24);
        return takeAll.visible() && ctx.backpack.select().count() < 28;
    }

    @Override
    public void execute() {
        System.out.println("LootAll.execute");
        ctx.input.send("{VK_SPACE down}");
        Condition.sleep(Random.nextInt(50,100));
        ctx.input.send("{VK_SPACE up}");
        AIOKiller.lastLoot = System.currentTimeMillis();
//        Condition.wait(new Callable<Boolean>() {
//            @Override
//            public Boolean call() throws Exception {
//                return !takeAll.visible();
//            }
//        }, 150, 5);
    }
}