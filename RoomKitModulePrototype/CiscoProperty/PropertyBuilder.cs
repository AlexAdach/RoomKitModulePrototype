using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RoomKitModulePrototype
{
    public class PropertyBuilder : IPropertyBuilder
    {
        private CiscoProperty _property;

        protected string name;
        protected string path;
        protected ICodecCommunication comms;
        protected List<XAPICommandBase> commands;


/*        public string Name { set => _name = value; }
        protected string Path { set => _path = value; }
        public ICommunication Comms { set => _comms = value; }*/
 
        public PropertyBuilder()
        {
        }
        public PropertyBuilderPath StartBuild(string name)
        {
            this.name = name;
            commands = new List<XAPICommandBase>();
            return new PropertyBuilderPath(this);
        }

        public class PropertyBuilderPath
        {
            PropertyBuilder _propertyBuilder;
            public PropertyBuilderPath(PropertyBuilder propertyBuilder)
            {
                _propertyBuilder = propertyBuilder;
            }
            public PropertyBuilderReference SetPath(string path)
            {
                _propertyBuilder.path = path;
                return new PropertyBuilderReference(_propertyBuilder);
            }
        }

        public class PropertyBuilderReference
        {
            PropertyBuilder _propertyBuilder;
            public PropertyBuilderReference(PropertyBuilder propertyBuilder)
            {
                _propertyBuilder = propertyBuilder;
            }
            public PropertyBuilderOptions SetReference(ICodecCommunication comms)
            {
                _propertyBuilder.comms = comms;
                return new PropertyBuilderOptions(_propertyBuilder);
            }
        }

        public class PropertyBuilderOptions
        {

            PropertyBuilder _propertyBuilder;

            public PropertyBuilderOptions(PropertyBuilder propertyBuilder)
            {
                _propertyBuilder = propertyBuilder;
            }

            public PropertyBuilderOptions AddCommand(string[] p, string res)
            {
                var cmd = new XCommand(_propertyBuilder.path, p, res);
                _propertyBuilder.commands.Add(cmd);
                return this;
            }

            public PropertyBuilderOptions AddToggle(string p)
            {
                var cmd = new XToggle(_propertyBuilder.path, p);
                _propertyBuilder.commands.Add(cmd);
                return this;
            }

            public PropertyBuilderOptions AddStatus(string p)
            {
                var cmd = new XStatus(_propertyBuilder.path, p);
                _propertyBuilder.commands.Add(cmd);
                return this;
            }

            public CiscoProperty Build()
            {
                var name = _propertyBuilder.name;
                var path = _propertyBuilder.path;
                var cmds = _propertyBuilder.commands;
                var comms = _propertyBuilder.comms;

                return new CiscoProperty(name, path, cmds, comms);
            }
        }
    }

    public interface IPropertyBuilder
    {
        PropertyBuilder.PropertyBuilderPath StartBuild(string name);

    }
}
