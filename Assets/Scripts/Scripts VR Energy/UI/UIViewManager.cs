using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VREnergy.UI
{
    /// <summary>
    /// Componente responsável por controlar a navegação e o histórico das <see cref="UIView"/>.
    /// <example>
    /// - Menu Canvas (UIViewManager)
    ///     - Main Menu (UIView)
    ///     - Settings (UIView)
    ///     - Credits (UIView)
    /// </example>
    /// </summary>
    public class UIViewManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Primeira página a ser carregada.")]
        private UIView startingView;
        
        [SerializeField, Tooltip("Caso vazio, será pego todos os filhos com o componente " + nameof(UIView) + ".")]
        private List<UIView> views;
    
        private readonly Stack<UIView> _viewHistory = new Stack<UIView>();
        private UIView _currentView;
    
        private void Awake()
        {
            SetupViews();
        }

        private void Start()
        {
            InitializeViews();
            NavegateTo(startingView);
        }

        /// <summary>
        /// Navega para a página dada.
        /// </summary>
        /// <param name="view">Página alvo</param>
        /// <param name="remember">Adicionar ao histórico?</param>
        public void NavegateTo(UIView view, bool remember = true)
        {
            if (view == null) return;
        
            if (_currentView != null)
            {
                if (remember)
                {
                    _viewHistory.Push(_currentView);
                }
                
                _currentView.Hide();
            }
        
            view.Show();
            _currentView = view;
        }

        /// <summary>
        /// Navega para a página, adicionando no histórico.
        /// Esse método tem como propósito ser chamado pelo editor.
        /// </summary>
        /// <param name="view">Página alvo</param>
        /// <see cref="NavegateTo"/>
        public void NavegateToWithHistory(UIView view) => NavegateTo(view);

        /// <summary>
        /// Navega para a página, sem adicionar no histórico.
        /// Esse método tem como propósito ser chamado pelo editor.
        /// </summary>
        /// <param name="view">Página alvo</param>
        /// <see cref="NavegateTo"/>
        public void NavegateToWithoutHistory(UIView view) => NavegateTo(view, false);

        /// <summary>
        /// Volta para a página anterior.
        /// </summary>
        public void GoBack()
        {
            if (_viewHistory.Count == 0) return;
        
            NavegateTo(_viewHistory.Pop());
        }

        private void SetupViews()
        {
            if (views.Count == 0)
            {
                views = GetComponentsInChildren<UIView>().ToList();

                if (views.Count == 0)
                {
                    Debug.LogWarning($"Could not find Views in {gameObject}.", this);
                }
            }
        }

        private void InitializeViews()
        {
            for (int i = 0; i < views.Count; i++)
            {
                views[i].Initialize(this);
                views[i].Hide();
            }
        }
    }
}
