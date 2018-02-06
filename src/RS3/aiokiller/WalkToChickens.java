package RS3.aiokiller;

import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Path;
import org.powerbot.script.rt6.TilePath;

import java.util.EnumSet;
import java.util.concurrent.Callable;

public class WalkToChickens extends RS3Task {
    final Tile path[] = {
            new Tile(3214, 3257, 0),
            new Tile(3215, 3260,0),
            new Tile(3216,3264,0),
            new Tile(3216,3268,0),
            new Tile(3215, 3272,0),
            new Tile(3213,3276,0),
            new Tile(3210,3279,0),
    };

    public WalkToChickens(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.backpack.select().count() == 0 &&
                ctx.players.local().tile().distanceTo(new Tile(3207,3283,0)) > 10;
    }

    @Override
    public void execute() {
        System.out.println("WalkToChickens.execute");
        TilePath tilePath = new TilePath(ctx, path).randomize(1,1);
        if (tilePath.next().tile().matrix(ctx).reachable() && tilePath.traverse(EnumSet.of(Path.TraversalOption.HANDLE_RUN, Path.TraversalOption.SPACE_ACTIONS))) {
            System.out.println("Traversing 1");
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return !ctx.players.local().inMotion() || ctx.movement.destination().distanceTo(ctx.players.local()) < 3;
                }
            }, 100, 5);
        }
//
//        tilePath = ctx.movement.newTilePath(new Tile(3206,3283,0)).randomize(2, 2);
//        if (tilePath.traverse(EnumSet.of(Path.TraversalOption.HANDLE_RUN, Path.TraversalOption.SPACE_ACTIONS))) {
//            System.out.println("Traversing 1");
//            Condition.wait(new Callable<Boolean>() {
//                @Override
//                public Boolean call() throws Exception {
//                    return !ctx.players.local().inMotion() || ctx.movement.destination().distanceTo(ctx.players.local()) < 10;
//                }
//            }, 100, 20);
//        }
    }
}
