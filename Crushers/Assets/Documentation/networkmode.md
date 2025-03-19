# Network Mode
lorem ipsum

```mermaid
sequenceDiagram
    user->>localClient: select vehicle
    localClient->>server : CmdSelectVehicle(GameObject vehicle)
    server->>server: set vehicle
    server->>localClient: vehicle set
    server->>otherClient: vehicle set
    localClient->>user: display ready button


```

```c#
    [Command]
    private void CmdSelectVehicle(GameObject vehicle){
        Debug.Log("Command Select: " + vehicle.name);
        
        PlayerVehiclePrefab = vehicle;

    }
```