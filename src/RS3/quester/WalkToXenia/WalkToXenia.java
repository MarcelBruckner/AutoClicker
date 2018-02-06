package RS3.quester.WalkToXenia;

import RS3.RS3Task;
import RS3.quester.startQuest.StartQuestTask;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.*;

public class WalkToXenia extends WalkToXeniaTask {

    public WalkToXenia(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.players.local().tile().floor() == 0 && ctx.players.local().tile().distanceTo(xeniaLoc) <= 50;
    }

    @Override
    public void execute() {
        LocalPath path = ctx.movement.findPath(xeniaLoc);
        path.traverse();
        Condition.sleep(Random.nextInt(300, 500));
    }
}
