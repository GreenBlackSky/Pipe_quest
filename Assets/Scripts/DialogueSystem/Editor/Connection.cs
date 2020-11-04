using System;
using UnityEditor;
using UnityEngine;
 

public class Connection {
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> RemoveConnection;
 
    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> RemoveConnection) {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.RemoveConnection = RemoveConnection;
        // HACK remove from here
        inPoint.connections.Add(this);
        outPoint.connections.Add(this);
        outPoint.replyParent.nextLineID = inPoint.parent.lineID;
    }
 
    public void Draw() {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
        );
 
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap)) {
            if (RemoveConnection != null) {
                RemoveConnection(this);
            }
        }
    }

    public void Destroy() {
        inPoint.connections.Remove(this);
        outPoint.replyParent.nextLineID = -1;
    }
}
