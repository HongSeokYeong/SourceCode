using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum EventType
{
    None,
    Event1,
    Event2,
    Event3,
    Event4
}

public class EventManager
{
    public delegate void EventHandler(BaseEventMsg msg);

    private Dictionary<MiniGameEventType, EventHandler> eventHandlerDic = new Dictionary<MiniGameEventType, EventHandler>();

    public void RegisterEvent(MiniGameEventType eventType, EventHandler eventHandler)
    {
        if (eventHandlerDic == null)
        {
            eventHandlerDic = new Dictionary<MiniGameEventType, EventHandler>();
        }

        if (eventHandlerDic.ContainsKey(eventType))
        {
            eventHandlerDic[eventType] += eventHandler;
        }
        else
        {
            eventHandlerDic.Add(eventType, eventHandler);
        }
    }

    public void UnRegisterEvent(MiniGameEventType eventType)
    {
        if (eventHandlerDic == null)
        {
            return;
        }

        if (eventHandlerDic.ContainsKey(eventType))
        {
            eventHandlerDic.Remove(eventType);
        }
    }

    public void UnRegisterEvent(MiniGameEventType eventType, EventHandler eventHandler)
    {
        if (eventHandlerDic == null)
        {
            return;
        }

        if (eventHandlerDic.ContainsKey(eventType))
        {
            eventHandlerDic[eventType] -= eventHandler;
        }
    }

    public async void SendEvent(MiniGameEventType eventType, BaseEventMsg msg, float delayTime = 0.0f, bool removeEvent = false)
    {
        if (eventHandlerDic == null)
        {
            return;
        }

        if (delayTime != 0.0f)
        {
            await Task.Delay((int)(delayTime * 1000));
        }

        if (eventHandlerDic.ContainsKey(eventType))
        {
            if (eventHandlerDic[eventType] != null)
            {
                eventHandlerDic[eventType].Invoke(msg);
            }
        }

        if(removeEvent)
        {
            UnRegisterEvent(eventType);
        }
    }

    public void SendEvent(MiniGameEventType eventType, float delayTime = 0.0f, bool removeEvent = false, params object[] inParams)
    {
        BaseEventMsg msg = new BaseEventMsg();

        msg.SetEventMsg(eventType, inParams);
        SendEvent(eventType, msg, delayTime, removeEvent);
    }
}

public class BaseEventMsg
{
    public MiniGameEventType msgType;
    public object[] eventParams = null;

    public BaseEventMsg()
    {

    }

    public BaseEventMsg(MiniGameEventType inMsgType, params object[] inParams)
    {
        MsgType = inMsgType;
        eventParams = inParams;
    }

    public void SetEventMsg(MiniGameEventType inMsgType, params object[] inParams)
    {
        MsgType = inMsgType;
        eventParams = inParams;
    }
}