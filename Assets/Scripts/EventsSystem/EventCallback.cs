using System;
using System.Collections;
using System.Collections.Generic;

public enum CallbackType {
    set_flag,
    unset_flag,

    start_listener,
    stop_listener,

    start_quest,
    progress_quest,
    end_quest,

    change_dialogue_initial_node,
    change_dialoge_connection,

    get_item,
    remove_item,

    load_level,
}

public class EventCallback {
    public CallbackType type;

    public void call() {

    }
}
