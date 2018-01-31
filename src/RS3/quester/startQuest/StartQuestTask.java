package RS3.quester.startQuest;

import RS3.RS3Task;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Component;

public abstract class StartQuestTask extends RS3Task {
    final int xenia = 9633;
    final Tile xeniaLoc = new Tile(3244, 3199, 0);
    final Component acceptButton = ctx.widgets.component(1500, 395);

    public StartQuestTask(ClientContext ctx) {
        super(ctx);
    }
}
