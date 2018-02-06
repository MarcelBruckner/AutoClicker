package RS3;

import org.powerbot.script.rt6.ClientAccessor;
import org.powerbot.script.rt6.ClientContext;

import java.util.ArrayList;
import java.util.List;

public abstract class RS3Task extends ClientAccessor {

    public List<RS3Task> tasks = new ArrayList<RS3Task>();

    public RS3Task(ClientContext ctx) {
        super(ctx);
    }

    public abstract boolean activate();

    public void execute(){
        for (RS3Task task : tasks) {
            if (task.activate()) {
                task.execute();
                break;
            }
        }
    }
}
