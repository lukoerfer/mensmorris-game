using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

using PropertyChanged;
using GalaSoft.MvvmLight.CommandWpf;

namespace MensMorris.Game.ViewModel
{
    /// <summary>
    /// View model for a screen message
    /// </summary>
    [ImplementPropertyChanged]
    public class ScreenMessageVM
    {
        /// <summary>
        /// Gets the current message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the color of the current message
        /// </summary>
        public Brush MessageColor { get; private set; }

        /// <summary>
        /// Gets the list of options of the current message
        /// </summary>
        public List<ScreenMessageOptionVM> Options { get; private set; }

        /// <summary>
        /// Gets whether a message is active at the moment
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.Message != string.Empty;
            }
        }

        /// <summary>
        /// Creates a new screen message view model
        /// </summary>
        public ScreenMessageVM()
        {
            this.Message = string.Empty;
            this.MessageColor = Brushes.Black;
            this.Options = new List<ScreenMessageOptionVM>();
        }

        /// <summary>
        /// Sets the current message, its color and the related options
        /// </summary>
        public void Set(string message, Brush color, params ScreenMessageOptionVM[] options)
        {
            this.Message = message;
            this.MessageColor = color;
            this.Options = options.ToList();
        }

        /// <summary>
        /// Resets the current message, its color and the related options
        /// </summary>
        public void Reset()
        {
            this.Message = string.Empty;
            this.MessageColor = Brushes.Black;
            this.Options = new List<ScreenMessageOptionVM>();
        }

    }

    /// <summary>
    /// View model for a screen message option
    /// </summary>
    public class ScreenMessageOptionVM
    {
        /// <summary>
        /// Gets or sets the description of the screen message option
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the command to choose the screen message option
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// Creates a new screen message option
        /// </summary>
        public ScreenMessageOptionVM(string description, Action action)
        {
            this.Description = description;
            this.Command = new RelayCommand(action);
        }
    }
}
