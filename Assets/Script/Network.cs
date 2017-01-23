using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RSG;
using SocketIO;

public class Network : MonoBehaviour{
    static Network instance;
    static IPromise<Network> connectionPromise;
    public SocketIOComponent socket;

    public static IPromise<Network> getInstance() {
        if (instance == null) {
            GameObject go = Instantiate(Resources.Load("Network")) as GameObject;
            instance = go.GetComponent<Network>();
            instance.socket = go.GetComponent<SocketIOComponent>();
            DontDestroyOnLoad(go);
            connectionPromise = new Promise<Network>((resolve, reject) => {
                instance.socket.On("init", (SocketIOEvent e) => {
                    Debug.Log("Socket connected.");
                    resolve(instance);
                });
            });
            DontDestroyOnLoad(go);
        }
        return connectionPromise;
    }

    public IPromise<string> callAction(string ns, string call, string data) {
        return new Promise<string>((resolve, reject) => {
            JSONObject jsonData = new JSONObject(data);
            JSONObject actionData = new JSONObject();
            actionData.AddField("namespace", ns);
            actionData.AddField("call", call);
            JSONObject requestData = new JSONObject();
            requestData.AddField("action", actionData);
            requestData.AddField("data", jsonData);

            socket.Emit("action", requestData, (response) => {
                response = response[0];
                var res = JsonUtility.FromJson<Protocol.ServerResponse>(response.ToString());
                if (res.success) {
                    if (response.HasField("data")) {
                        resolve(response.GetField("data").ToString());
                    } else {
                        resolve("");
                    }
                } else if (res.error != null) {
                    reject(new Exception(res.error));
                } else {
                    reject(new Exception());
                }
            });
        });
    }

    public IPromise<T> callAction<T>(string ns, string call, string data) {
        return new Promise<T>((resolve, reject) => {
            callAction(ns, call, data)
                .Then(res => {
                    resolve(JsonUtility.FromJson<T>(res));
                })
                .Catch(e => {
                    reject(e);
                });
        });
    }

    public IPromise<string> callAction<T>(string ns, string call, T data) {
        return callAction(ns, call, JsonUtility.ToJson(data));
    }

    public IPromise<T2> callAction<T1, T2>(string ns, string call, T1 data) {
        return new Promise<T2>((resolve, reject) => {
            callAction(ns, call, JsonUtility.ToJson(data))
                .Then(res => {
                    resolve(JsonUtility.FromJson<T2>(res));
                })
                .Catch(e => {
                    reject(e);
                });
        });
    }

    public IPromise<string> callAction(string ns, string call) {
        return callAction(ns, call, "");
    }

    public IPromise<T> callAction<T>(string ns, string call) {
        return new Promise<T>((resolve, reject) => {
            callAction(ns, call, "")
                .Then(res => {
                    resolve(JsonUtility.FromJson<T>(res));
                })
                .Catch(e => {
                    reject(e);
                });
        });
    }
}