package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.GameObject;

import java.util.concurrent.Callable;

public class Bank extends RS3Task {
    final int CHEST = 79036;

    public Bank(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.players.local().tile().distanceTo(ctx.bank.nearest().tile()) < 5 &&
                ctx.backpack.items().length > 0;
    }

    @Override
    public void execute() {
        System.out.println("Bank.execute");
        if(!ctx.bank.opened()) {
            ctx.bank.open();

            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return ctx.bank.opened();
                }
            },150,3);
        }
        ctx.bank.depositInventory();
        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return ctx.backpack.select().count() == 0;
            }
        },150,10);
        AIOKiller.runs++;
        ctx.bank.close();
        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !ctx.bank.opened();
            }
        },150,5);

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
