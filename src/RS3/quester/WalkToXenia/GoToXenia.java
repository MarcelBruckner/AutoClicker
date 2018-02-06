package RS3.quester.WalkToXenia;

import RS3.Quester;
import RS3.RS3Task;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;

public class GoToXenia extends RS3Task {
    final Tile xeniaLoc = new Tile(3244, 3199, 0);

    public GoToXenia(ClientContext ctx) {
        super(ctx);

        tasks.add(new LumbridgeTeleport(ctx));
        tasks.add(new WalkToXenia(ctx));
    }

    @Override
    public boolean activate() {
        return Quester.progress == 0 && ctx.players.local().tile().distanceTo(xeniaLoc) > 8;
    }
}
