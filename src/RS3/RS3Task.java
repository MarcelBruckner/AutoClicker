package RS3;

import org.powerbot.script.rt6.ClientAccessor;
import org.powerbot.script.rt6.ClientContext;

import java.util.ArrayList;
import java.util.List;

public abstract class RS3Task extends ClientAccessor {

    boolean finished = false;

    public RS3Task(ClientContext ctx) {
        super(ctx);
    }

    public abstract boolean activate();
    public abstract void execute();
}
