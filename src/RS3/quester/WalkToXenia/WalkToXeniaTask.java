package RS3.quester.WalkToXenia;

import RS3.RS3Task;
import RS3.aiokiller.WalkToChickens;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;

public abstract class WalkToXeniaTask extends RS3Task {
    final Tile xeniaLoc = new Tile(3244, 3199, 0);

    public WalkToXeniaTask(ClientContext ctx) {
        super(ctx);
    }
}
