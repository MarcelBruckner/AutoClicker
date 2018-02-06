package RS3.quester.enterCatacombs;

import RS3.Quester;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.GameObject;

import java.util.concurrent.Callable;

public class EnterCatacombs extends RS3Task {
    public EnterCatacombs(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return Quester.progress == 1 && !ctx.players.local().inMotion();
    }

    @Override
    public void execute() {
        final GameObject gate = ctx.objects.select().id(48797).nearest().poll();

        if (ctx.players.local().tile().floor() == 0 && gate.inViewport() || Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                ctx.camera.turnTo(gate);
                return gate.inViewport();
            }
        }, 150, 5)) {
            gate.interact("Enter");
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return !ctx.players.local().inMotion();
                }
            }, 150, 10);
        }else if(ctx.chat.canContinue()) {
            ctx.chat.clickContinue();
            Condition.sleep(Random.nextInt(200, 300));
        }else{
            Quester.progress = -1;
        }
    }
}
