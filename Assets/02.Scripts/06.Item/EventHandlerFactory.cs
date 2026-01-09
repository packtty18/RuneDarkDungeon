using System.Collections.Generic;
using UnityEngine;

public class EventHandlerFactory
{
    private readonly UpgradeManager _upgradeManager;
    private readonly UI_Tooltip _tooltip;
    private readonly UI_DragIcon _dragIcon;
    private readonly UI_Background[] _backgrounds;

    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    public EventHandlerFactory(UpgradeManager upgradeManager, UI_Tooltip tooltip, UI_DragIcon dragIcon,
        UI_Background[] backgrounds)
    {
        _upgradeManager = upgradeManager;
        _tooltip = tooltip;
        _dragIcon = dragIcon;
        _backgrounds = backgrounds;

        _handlerDict = new()
        {
            { EInventoryMode.Normal, new SwapEventHandler(_tooltip, _dragIcon, _backgrounds)},
            { EInventoryMode.Upgrade , new RegisterEventHandler(_upgradeManager)}
        };
    }
}
