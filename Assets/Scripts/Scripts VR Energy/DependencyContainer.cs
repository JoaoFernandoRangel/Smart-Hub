using System;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

/// <summary>
/// Classe responsável por providenciar todas as dependencias do projeto
/// </summary>
[DefaultExecutionOrder(-25)]
public class DependencyContainer : MonoBehaviour
{
    private static DependencyContainer s_instance;

    public static DependencyContainer Instance => s_instance;

    [SerializeField] private MonoBehaviour[] dependencies;
    
    private Dictionary<Type, object> _container;

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Initialization();
    }

    private void Initialization()
    {
        _container = new Dictionary<Type, object>();
        
        foreach (var dependency in dependencies)
        {
            Register(dependency, dependency.GetType());
        }
        
        Register<IProcedureRepository>(new LocalJsonProcedureRepository());
        Register<IProcedureService>(new ProcedureService(Get<IProcedureRepository>()));
        Register<ISceneRepository>(new LocalJsonSceneRepository());
        Register<ISceneService>(new SceneService(Get<ISceneRepository>()));
    }
    
    private void Register(object instance, Type type)
    {
        if (!type.IsInstanceOfType(instance))
        {
            Debug.LogError($"Instance '{instance}' is not Type of '{type}'.");
            return;
        }
        
        if (!_container.ContainsKey(type))
        {
            _container.Add(type, instance);
        }
        else
        {
            Debug.LogError($"Type '{type}' already exists.");
        }
    }
    
    /// <summary>
    /// Cadastra a instância de um tipo.
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    public void Register<T>(T instance)
    {
        Register(instance, typeof(T));
    }

    /// <summary>
    /// Retorna a instância de tipo cadastrado.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>()
    {
        return (T)_container[typeof(T)];
    }
}
