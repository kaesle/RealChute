﻿using UnityEngine;
using RealChute.Extensions;
using RealChute.Libraries;

/* RealChute was made by Christophe Savard (stupid_chris) and is licensed under CC-BY-NC-SA. You can remix, modify and
 * redistribute the work, but you must give attribution to the original author (me) and you cannot sell your derivatives.
 * For more information contact me on the forum. */

namespace RealChute
{
    public class RealChuteSettings
    {
        #region Fetch
        private static RealChuteSettings _fetch = null;
        /// <summary>
        /// Returns the current RealChute_Settings config file
        /// </summary>
        public static RealChuteSettings fetch
        {
            get
            {
                if (_fetch == null) { _fetch = new RealChuteSettings(); }
                return _fetch;
            }
        }
        #endregion

        #region Propreties
        private bool _autoArm = false;
        /// <summary>
        /// If parachutes must automatically arm when staged
        /// </summary>
        public bool autoArm
        {
            get { return this._autoArm; }
            set { this._autoArm = value; }
        }

        private bool _jokeActivated = false;
        /// <summary>
        /// If April Fools joke is activated
        /// </summary>
        public bool jokeActivated
        {
            get { return this._jokeActivated; }
            set { this._jokeActivated = value; }
        }

        private bool _useStaging = true;
        /// <summary>
        /// If parachutes can be activated through staging
        /// </summary>
        public bool useStaging
        {
            get { return this._useStaging; }
            set { this._useStaging = value; }
        }

        private Vector2 _position = new Vector2(100, 100);
        /// <summary>
        /// Position vector of the window
        /// </summary>
        public Vector2 position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        private bool _hideIcon = false;
        /// <summary>
        /// If the SpaceCenter icon is hidden or not
        /// </summary>
        public bool hideIcon
        {
            get { return this._hideIcon; }
            set { this._hideIcon = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Loads the RealChute_Settings config to memory
        /// </summary>
        public RealChuteSettings()
        {
            ConfigNode node = new ConfigNode(), settings = new ConfigNode("REALCHUTE_SETTINGS");
            Debug.Log("[RealChute]: Loading settings file.");
            if (!System.IO.File.Exists(RCUtils.settingsURL))
            {
                Debug.LogWarning("[RealChute]: RealChute_Settings.cfg is missing component. Creating new version.");
                settings.AddValue("autoArm", autoArm);
                settings.AddValue("jokeActivated", jokeActivated);
                settings.AddValue("useStaging", useStaging);
                settings.AddValue("hideIcon", hideIcon);
                node.AddNode(settings);
                node.Save(RCUtils.settingsURL);
            }
            else
            {
                node = ConfigNode.Load(RCUtils.settingsURL);
                node.TryGetNode("REALCHUTE_SETTINGS", ref settings);
                bool missing = false;
                if (!settings.TryGetValue("autoArm", ref _autoArm)) { missing = true; }
                if (!settings.TryGetValue("jokeActivated", ref _jokeActivated)) { missing = true; }
                if (settings.TryGetValue("useStaging", ref _useStaging)) { missing = true; }
                if (settings.TryGetValue("hideIcon", ref _hideIcon)) { missing = true; }
                if (missing)
                {
                    Debug.LogWarning("[RealChute]: RealChute_Settings.cfg is missing component. Fixing settings file.");
                    settings.ClearValues();
                    settings.AddValue("autoArm", autoArm);
                    settings.AddValue("jokeActivated", jokeActivated);
                    settings.AddValue("useStaging", useStaging);
                    settings.AddValue("hideIcon", hideIcon);
                    node.ClearData();
                    node.AddNode(settings);
                    node.Save(RCUtils.settingsURL);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves the RealChute_Settings config into GameData
        /// </summary>
        public static void SaveSettings()
        {
            ConfigNode settings = new ConfigNode("REALCHUTE_SETTINGS"), node = new ConfigNode();
            settings.AddValue("autoArm", fetch.autoArm);
            settings.AddValue("jokeActivated", fetch.jokeActivated);
            settings.AddValue("useStaging", fetch.useStaging);
            settings.AddValue("hideIcon", fetch.hideIcon);
            if (PresetsLibrary.instance.presets.Count > 0)
            {
                PresetsLibrary.instance.presets.ForEach(p => settings.AddNode(p.Save()));
            }
            node.AddNode(settings);
            node.Save(RCUtils.settingsURL);
            Debug.Log("[RealChute]: Saved settings file.");
        }
        #endregion
    }
}