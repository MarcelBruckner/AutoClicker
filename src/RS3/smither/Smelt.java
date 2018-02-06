package RS3.smither;

import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.Tile;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.GameObject;
import org.powerbot.script.rt6.TilePath;
import z.Con;

import java.util.concurrent.Callable;

public class Smelt extends RS3Task {
    private int items[];
    private int processor;
    private int makeable[];
    private int makeIfMoreThan[];
    private boolean smelt;

    public Smelt(ClientContext ctx, boolean smelt, int[] withdrawn, int processor, int makeable[], int makeIfMoreThan[]) {
        super(ctx);
        this.smelt = smelt;
        this.items = withdrawn;
        this.processor = processor;
        this.makeable = makeable;
        this.makeIfMoreThan = makeIfMoreThan;
    }

    @Override
    public boolean activate() {
        return !ctx.productionInterface.working() && ctx.backpack.select().id(items).count() >= makeIfMoreThan[makeIfMoreThan.length - 1] && !ctx.players.local().inMotion();
    }

    public void execute(){
        final GameObject furnance = ctx.objects.select().id(processor).nearest().poll();

        if (!ctx.productionInterface.opened() && (furnance.inViewport() || Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                if(ctx.camera.pitch() > 40 || ctx.camera.pitch() < 30)
                    ctx.camera.pitch(Random.nextInt(30,40));
                ctx.camera.turnTo(furnance);
                return furnance.inViewport();
            }
        }, 150, 20))) {
            if (furnance.interact(smelt ? "Smelt" : "Smith", smelt ? "Furnance" : "Anvil")) {
                Condition.wait(new Callable<Boolean>() {
                    @Override
                    public Boolean call() throws Exception {
                        return !ctx.players.local().inMotion() || ctx.productionInterface.opened();
                    }
                }, 150, 20);
            }
        }

        if( ctx.productionInterface.opened()) {
            for (int i = 0; i < makeable.length; i++) {
                if (ctx.backpack.select().id(items[0]).count() >= makeIfMoreThan[i]) {
                    ctx.productionInterface.selectItem(makeable[i]);
                    final int finalI = i;
                    Condition.wait(new Callable<Boolean>() {
                        @Override
                        public Boolean call() throws Exception {
                            return ctx.productionInterface.selectedItemId() == makeable[finalI];
                        }
                    }, 50, 20);
                    ctx.productionInterface.makeItem(true);
                    break;
                }
            }
            if(ctx.productionInterface.opened())
                ctx.productionInterface.close();
        }
    }
}
