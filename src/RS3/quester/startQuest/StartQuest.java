package RS3.quester.startQuest;

import RS3.Quester;
import RS3.RS3Task;
import RS3.quester.WalkToXenia.GoToXenia;
import org.powerbot.script.rt6.ClientContext;

public class StartQuest extends RS3Task {
    public StartQuest(ClientContext ctx) {
        super(ctx);
        tasks.add(new AcceptQuest(ctx));
        tasks.add(new TalkToXenia(ctx));
    }

    @Override
    public boolean activate() {
        return Quester.progress == 0;
    }
}
