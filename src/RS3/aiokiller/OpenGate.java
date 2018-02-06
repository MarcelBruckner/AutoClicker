package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.GameObject;
import org.powerbot.script.rt6.Interactive;
import org.powerbot.script.rt6.Objects;

import java.util.concurrent.Callable;

public class OpenGate extends RS3Task {

    final int GATE[] = {45208, 45206};

    int tries = 0;

    public OpenGate(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return AIOKiller.gateClosed &&
                ctx.players.local().tile().distanceTo(ctx.objects.select().id(GATE).nearest().poll().tile()) < 10 &&
                ctx.objects.select().id(GATE).nearest().poll().orientation() == 3;
    }

    @Override
    public void execute() {
        System.out.println("OpenGate.execute");
        if(tries++ >= 1){
            ctx.camera.angle(Random.nextInt(0, 360));
        }

        if(Math.abs(ctx.camera.yaw() - 270) < 15) {
            System.out.println(ctx.camera.angle(Random.nextInt(200, 250)));

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return Math.abs(ctx.camera.yaw() - 270) > 15;
                }
            }, 150, 5);
        }

        final GameObject gate = ctx.objects.select().id(GATE).nearest().poll();
        BringToViewPort(gate);
        if(gate.interact("Open")) {
            tries = 0;
            AIOKiller.gateClosed = false;
        }

        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !gate.valid();
            }
        }, 150, 20);
    }

    private void BringToViewPort(final GameObject obj){
        if(!obj.inViewport()){
            ctx.camera.turnTo(obj);
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return obj.inViewport();
                }
            }, 150, 5);
        }
    }
}
