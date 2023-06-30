using System;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Interface padrão para toda implementação de chave.
/// </summary>
public interface IKey
{
    LockType GetLockType();
    
    [Obsolete("isLockedOnSocket está obsoleto, use IsLockedOnSocket em vez disso.")]
    bool isLockedOnSocket();
    
    /// <summary>
    /// Controla se a chave está incapacitada de sair do socket.
    /// </summary>
    bool IsLockedOnSocket { get; set; }
    
    /// <summary>
    /// Conecta a chave no socket.
    /// </summary>
    /// <param name="socket">O socket no qual a chave irá conectar.</param>
    void Connect(XRSocketInteractor socket);
    
    /// <summary>
    /// Desconecta a chave do socket.
    /// </summary>
    void Disconnect();
    
    /// <summary>
    /// Evento lançado quando ocorre interação com a chave.
    /// </summary>
    event Action<bool> OnKeyActivation;
    
    /// <summary>
    /// Evento lançado quando a chave conecta no socket.
    /// </summary>
    event Action OnConnect;
    
    /// <summary>
    /// Evento lançado quando a chave desconecta do socket.
    /// </summary>
    event Action OnDisconnect;
    
    bool CanConnect();
}
