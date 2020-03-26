package RS3.quester.WalkToXenia;

import RS3.RS3Task;
import RS3.quester.startQuest.StartQuestTask;
import org.powerbot.script.Condition;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Component;

import java.util.concurrent.Callable;

public class LumbridgeTeleport extends WalkToXeniaTask {

    Component teleportH = ctx.widgets.component(1465, 18);
    Component lumbridge = ctx.widgets.component(1092, 17);

    public LumbridgeTeleport(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.players.local().tile().distanceTo(xeniaLoc) > 50 &&
                ctx.players.local().animation() == -1;
    }

    @Override
    public void execute() {
        teleportH.click();
        lumbridge.click();

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return ctx.players.local().animation() != -1;
            }
        }, 300, 10);
    }
}