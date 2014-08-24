﻿using System;
using Assets.Scripts.Map;
using Mercraft.Core.Scene;
using Mercraft.Explorer.Interactions;
using Mercraft.Infrastructure.Bootstrap;

namespace Assets.Scripts.Character
{
    public class DemoBootstrapper: BootstrapperPlugin
    {
        private CompositeModelBehaviour _solidModelBehavior;

        private CompositeModelBehaviour _waterModelBehavior;

        public override string Name
        {
            get { return "demo"; }
        }

        public override bool Run()
        {
            // NOTE we should keep reference to prevent GC as RegisterInstance uses WeakReference
            // TODO add ability to register object without this trick
            _solidModelBehavior = new CompositeModelBehaviour("solid", new Type[]
            {
                typeof (DestroyableObject),
                typeof (LocationInfoHolder),
            });

            Container.RegisterInstance<IModelBehaviour>(_solidModelBehavior, "solid");

            _waterModelBehavior = new CompositeModelBehaviour("water", new Type[]
            {
                typeof(WaterSimple)
            });

            Container.RegisterInstance<IModelBehaviour>(_waterModelBehavior, "water");

            return true;
        }
    }
}
