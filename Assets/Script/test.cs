using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    void Start() {
        var login = new Protocol.Out.Auth.Login.Request();
        login.username = "root";
        login.password = "root";
        Network.getInstance().
            Then(network => {
                network.callAction("auth", "login", login)
                    .Then(res => {
                        Debug.Log(res);
                    })
                    .Catch(e => {
                        Debug.LogError(e);
                    });
            })
            .Catch(e => {

            });
    }
}
