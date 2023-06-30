using System;

public enum LockType
{
    Cadeado_Chave01,
    Painel_Cadeado,
    Fechadura_caixa_Primaria,
    Fechadura_Painel,
    Socket_chave_Manobra,
    TravaDijuntor,
    PlugPainel,
    Bireta,
    Socket_Chave_Biela,
    PortinholaCima,
    PortinholaBaixo
}

/// <summary>
/// Interface padrão para toda implementação de fechadura que possui uma chave.
/// </summary>
public interface ILock
{
    /// <summary>
    /// Evento lançado quando a fechadura for trancada.
    /// </summary>
    event Action onLock;
    
    /// <summary>
    /// Evento lançado quando a fechadura for destrancada.
    /// </summary>
    event Action onUnlock;
    
    /// <summary>
    /// Evento lançado quando a chave conecta no socket.
    /// </summary>
    event Action onKeyIn;
    
    /// <summary>
    /// Evento lançado quando a chave sai do socket.
    /// </summary>
    event Action onKeyOut;
    
    /// <summary>
    /// Diz se a chave está no socket.
    /// </summary>
    bool IsKeyPlaced { get; }
    
    bool isOpen();
    
    /// <summary>
    /// Conecta a chave no socket.
    /// </summary>
    /// <returns></returns>
    bool PlaceKey();
    
    /// <summary>
    /// Remove a chave do socket.
    /// </summary>
    /// <returns></returns>
    bool RemoveKey();
    
    /// <summary>
    /// Tranca o socket.
    /// </summary>
    void Lock();
    
    /// <summary>
    /// Destranca o socket.
    /// </summary>
    void Unlock();
}
