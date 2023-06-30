using System;

namespace VREnergy.PRO
{
    /// <summary>
    /// Contrato que define a implementação de todos os assets que serão identificados
    /// pelo controlador de passos.
    /// </summary>
    public interface IPROAsset
    {
        /// <summary>
        /// Identificador do asset.
        /// </summary>
        string UnityId { get; set; }
        
        /// <summary>
        /// Diz se o asset está ativo no passo atual.
        /// </summary>
        bool IsAssetActive { get; }
        
        /// <summary>
        /// Evento na interação de um asset.
        /// </summary>
        event Action<PROAction> OnAssetInteraction;

        void AssetInteraction(PROAction proAction);
        
        /// <summary>
        /// Este método será chamado pelo controlador de passos do procedimento
        /// quando o asset estiver no passo que será executado no momento.
        /// </summary>
        void EnableAsset();
        
        /// <summary>
        /// Este método será chamado pelo controlador de passos do procedimento
        /// quando for dado sequência para o próximo passo.
        /// </summary>
        void DisableAsset();
    }
}
