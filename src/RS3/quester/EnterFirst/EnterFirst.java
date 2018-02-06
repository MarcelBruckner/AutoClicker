package RS3.quester.EnterFirst;

import RS3.Quester;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.LocalPath;
import org.powerbot.script.rt6.Path;
import org.powerbot.script.rt6.TilePath;

import javax.management.QueryEval;
import java.util.EnumSet;
import java.util.concurrent.Callable;

public class EnterFirst extends RS3Task {
    public EnterFirst(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return Quester.progress == 2 && !ctx.players.local().inMotion();
    }

    @Override
    public void execute(){
        System.out.println("EnterFirst.execute");
        TilePath tilePath = new TilePath(ctx, new Tile[]{new Tile(10149, 4510, 1)}).randomize(1,1);
        if (tilePath.next().tile().matrix(ctx).reachable() && tilePath.randomize(1,1).traverse(EnumSet.of(Path.TraversalOption.HANDLE_RUN, Path.TraversalOption.SPACE_ACTIONS))) {
            System.out.println("Traversing 1");
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return !ctx.players.local().inMotion() || ctx.movement.destination().distanceTo(ctx.players.local()) < 3;
                }
            }, 100, 20);
        }
        Quester.progress = -1;
    }
}
