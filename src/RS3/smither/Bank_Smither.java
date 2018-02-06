package RS3.smither;

import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.*;

import java.util.EnumSet;
import java.util.concurrent.Callable;

public class Bank_Smither extends RS3Task {

    private int items[];
    private int amount[];

    public Bank_Smither(ClientContext ctx, int[] items, int[] amount) {
        super(ctx);
        this.items = items;
        this.amount = amount;
    }

    @Override
    public boolean activate() {
        return !ctx.productionInterface.working() && !ctx.players.local().inMotion();
    }

    public void execute() {
        Tile t = new Tile(3213, 3257, 0);
        TilePath p = new TilePath(ctx, new Tile[]{t}).randomize(1, 1);

        if (!ctx.bank.opened() &&
                (ctx.players.local().tile().distanceTo(t) < 3 ||
                        p.traverse(EnumSet.of(Path.TraversalOption.HANDLE_RUN, Path.TraversalOption.SPACE_ACTIONS)))) {
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return !ctx.players.local().inMotion() || ctx.movement.destination().distanceTo(ctx.players.local()) < 3 || ctx.bank.inViewport();
                }
            }, 150, 20);
            if (ctx.bank.open()) {
                Condition.wait(new Callable<Boolean>() {
                    @Override
                    public Boolean call() throws Exception {
                        return ctx.bank.opened() || !ctx.players.local().inMotion();
                    }
                }, 150, 20);
            }
        }

        if (ctx.bank.opened()) {
            ctx.bank.depositInventory();

            if (ctx.bank.select().id(items).count() == 0) {
                ctx.controller.stop();
            }

            for (int i = 0; i < items.length; i++) {
                ctx.bank.withdraw(items[i], amount[i]);
            }
//            ctx.bank.presetGear1();

            ctx.bank.close();

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return !ctx.bank.opened();
                }
            }, 150, 20);
        }
    }
}
