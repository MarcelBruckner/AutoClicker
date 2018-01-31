package RS3.quester.startQuest;

import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.*;

public class WalkToXenia extends StartQuestTask {

    public WalkToXenia(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.players.local().tile().distanceTo(xeniaLoc) > 3 &&
                ctx.players.local().tile().distanceTo(xeniaLoc) <= 50 &&
                !acceptButton.visible();
    }

    @Override
    public void execute() {
        LocalPath path = ctx.movement.findPath(xeniaLoc);
        path.traverse();
        Condition.sleep(Random.nextInt(300,500));
    }
}
