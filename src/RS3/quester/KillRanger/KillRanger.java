package RS3.quester.KillRanger;

import RS3.Quester;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Npc;
import z.Con;

import java.util.concurrent.Callable;

public class KillRanger extends RS3Task {
    public KillRanger(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return Quester.progress == 3 && !ctx.players.local().inCombat() && ctx.players.local().animation() == -1;
    }

    @Override
    public void execute(){
        final Npc ranger = ctx.npcs.select().id(9628).poll();

        if(ranger.inViewport() || Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                ctx.camera.turnTo(ranger);
                ctx.movement.step(ranger);
                return ranger.inViewport();
            }
        }, 150, 5)){
            ranger.interact("KillRanger");
        }
    }
}
