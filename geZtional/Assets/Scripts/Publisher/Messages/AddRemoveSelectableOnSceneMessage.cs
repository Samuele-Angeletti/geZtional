using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveSelectableOnSceneMessage : IPublisherMessage
{
    public bool AddingNewSelectable;
    public Selectable Selectable;
    public AddRemoveSelectableOnSceneMessage(bool addingNewSelectable, Selectable selectable)
    {
        AddingNewSelectable = addingNewSelectable;
        Selectable = selectable;
    }
}
