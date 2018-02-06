package RS3.aiokiller;

import RS3.AIOKiller;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Component;
import org.powerbot.script.rt6.Item;
import z.Con;

import java.util.Dictionary;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.concurrent.Callable;

public class CountBackpack extends RS3Task {

    final int ITEMS[] = {314, 2138, 1994, 526};
    final int VALUES[] = new int[ITEMS.length];

    Dictionary<Integer, Integer> values = new Hashtable<Integer, Integer>();

    Component lastLine = ctx.widgets.component(137, 76).component(0);

    long lastExec = 0;

    public CountBackpack(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return ctx.backpack.select().count() == 28 && AIOKiller.backpackValue == 0;
    }

    @Override
    public void execute() {
        AIOKiller.backpackValue = 0;
        Iterator items = ctx.backpack.select().iterator();

        while(items.hasNext()){
            Item item = (Item)items.next();
            System.out.println(item.name());
            if(values.get(item.id()) == null) {
                try {
                    String raw = lastLine.text();
                    System.out.println("Last line: " + raw);

                    item.interact("Examine");

                    final String finalRaw = raw;
                    if(!lastLine.text().equals(raw)) {
                        System.out.println("Innnnnnnnnnn");
                        Condition.wait(new Callable<Boolean>() {
                            @Override
                            public Boolean call() throws Exception {
                                String tmp = lastLine.text();
                                System.out.println("New Line: " + tmp);
                                return !tmp.equals(finalRaw);
                            }
                        }, 150, 10);
                    }
                    raw = lastLine.text();

                    Condition.sleep(Random.nextInt(1000,2000));
                    raw = raw.substring(0, raw.length() - " gp each".length());
                    String split[] = raw.split(": ");
                    raw = split[split.length - 1];
                    values.put(item.id(), Integer.parseInt(raw));
                }catch (NumberFormatException e){
                    System.out.println("NumberFormatException");
                }
            }

            if(values.get(item.id()) != null) {
                AIOKiller.backpackValue += values.get(item.id()) * item.stackSize();
            }
        }

        lastExec = System.currentTimeMillis();
    }
}
