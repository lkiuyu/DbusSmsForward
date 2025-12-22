namespace ModemManager1.DBus
{
    using System;
    using Tmds.DBus.Protocol;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tmds.DBus;

    partial class ObjectManager : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.DBus.ObjectManager";
        public ObjectManager(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>> GetManagedObjectsAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aeoaesaesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "GetManagedObjects");
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchInterfacesAddedAsync(Action<Exception?, (ObjectPath ObjectPath, Dictionary<string, Dictionary<string, VariantValue>> InterfacesAndProperties)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "InterfacesAdded", (Message m, object? s) => ReadMessage_oaesaesv(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchInterfacesRemovedAsync(Action<Exception?, (ObjectPath ObjectPath, string[] Interfaces)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "InterfacesRemoved", (Message m, object? s) => ReadMessage_oas(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
    }
    record ModemManager1Properties
    {
        public string Version { get; set; } = default!;
    }
    partial class ModemManager1 : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1";
        public ModemManager1(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task ScanDevicesAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ScanDevices");
                return writer.CreateMessage();
            }
        }
        public Task SetLoggingAsync(string level)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetLogging");
                writer.WriteString(level);
                return writer.CreateMessage();
            }
        }
        public Task ReportKernelEventAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "ReportKernelEvent");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public Task InhibitDeviceAsync(string uid, bool inhibit)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "sb",
                    member: "InhibitDevice");
                writer.WriteString(uid);
                writer.WriteBool(inhibit);
                return writer.CreateMessage();
            }
        }
        public Task SetVersionAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Version");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task<string> GetVersionAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Version"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<ModemManager1Properties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static ModemManager1Properties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<ModemManager1Properties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<ModemManager1Properties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<ModemManager1Properties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Version": invalidated.Add("Version"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static ModemManager1Properties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new ModemManager1Properties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Version":
                        reader.ReadSignature("s");
                        props.Version = reader.ReadString();
                        changedList?.Add("Version");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record LocationProperties
    {
        public uint Capabilities { get; set; } = default!;
        public uint SupportedAssistanceData { get; set; } = default!;
        public uint Enabled { get; set; } = default!;
        public bool SignalsLocation { get; set; } = default!;
        public Dictionary<uint, VariantValue> Location { get; set; } = default!;
        public string SuplServer { get; set; } = default!;
        public string[] AssistanceDataServers { get; set; } = default!;
        public uint GpsRefreshRate { get; set; } = default!;
    }
    partial class Location : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Location";
        public Location(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task SetupAsync(uint sources, bool signalLocation)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ub",
                    member: "Setup");
                writer.WriteUInt32(sources);
                writer.WriteBool(signalLocation);
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<uint, VariantValue>> GetLocationAsyncMethod1()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aeuv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "GetLocation");
                return writer.CreateMessage();
            }
        }
        public Task SetSuplServerAsyncMethod1(string supl)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetSuplServer");
                writer.WriteString(supl);
                return writer.CreateMessage();
            }
        }
        public Task InjectAssistanceDataAsync(byte[] data)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ay",
                    member: "InjectAssistanceData");
                writer.WriteArray(data);
                return writer.CreateMessage();
            }
        }
        public Task SetGpsRefreshRateAsyncMethod1(uint rate)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetGpsRefreshRate");
                writer.WriteUInt32(rate);
                return writer.CreateMessage();
            }
        }
        public Task SetCapabilitiesAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Capabilities");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedAssistanceDataAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedAssistanceData");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEnabledAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Enabled");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSignalsLocationAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SignalsLocation");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetLocationAsync(Dictionary<uint, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Location");
                writer.WriteSignature("a{uv}");
                WriteType_aeuv(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetSuplServerAsyncMethod(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SuplServer");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetAssistanceDataServersAsync(string[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("AssistanceDataServers");
                writer.WriteSignature("as");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetGpsRefreshRateAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("GpsRefreshRate");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<uint> GetCapabilitiesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Capabilities"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetSupportedAssistanceDataAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedAssistanceData"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetEnabledAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Enabled"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<bool> GetSignalsLocationAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SignalsLocation"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<uint, VariantValue>> GetLocationAsyncMethod2()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Location"), (Message m, object? s) => ReadMessage_v_aeuv(m, (ModemManager1Object)s!), this);
        public Task<string> GetSuplServerAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SuplServer"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string[]> GetAssistanceDataServersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "AssistanceDataServers"), (Message m, object? s) => ReadMessage_v_as(m, (ModemManager1Object)s!), this);
        public Task<uint> GetGpsRefreshRateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "GpsRefreshRate"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<LocationProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static LocationProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<LocationProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<LocationProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<LocationProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Capabilities": invalidated.Add("Capabilities"); break;
                        case "SupportedAssistanceData": invalidated.Add("SupportedAssistanceData"); break;
                        case "Enabled": invalidated.Add("Enabled"); break;
                        case "SignalsLocation": invalidated.Add("SignalsLocation"); break;
                        case "Location": invalidated.Add("Location"); break;
                        case "SuplServer": invalidated.Add("SuplServer"); break;
                        case "AssistanceDataServers": invalidated.Add("AssistanceDataServers"); break;
                        case "GpsRefreshRate": invalidated.Add("GpsRefreshRate"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static LocationProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new LocationProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Capabilities":
                        reader.ReadSignature("u");
                        props.Capabilities = reader.ReadUInt32();
                        changedList?.Add("Capabilities");
                        break;
                    case "SupportedAssistanceData":
                        reader.ReadSignature("u");
                        props.SupportedAssistanceData = reader.ReadUInt32();
                        changedList?.Add("SupportedAssistanceData");
                        break;
                    case "Enabled":
                        reader.ReadSignature("u");
                        props.Enabled = reader.ReadUInt32();
                        changedList?.Add("Enabled");
                        break;
                    case "SignalsLocation":
                        reader.ReadSignature("b");
                        props.SignalsLocation = reader.ReadBool();
                        changedList?.Add("SignalsLocation");
                        break;
                    case "Location":
                        reader.ReadSignature("a{uv}");
                        props.Location = ReadType_aeuv(ref reader);
                        changedList?.Add("Location");
                        break;
                    case "SuplServer":
                        reader.ReadSignature("s");
                        props.SuplServer = reader.ReadString();
                        changedList?.Add("SuplServer");
                        break;
                    case "AssistanceDataServers":
                        reader.ReadSignature("as");
                        props.AssistanceDataServers = reader.ReadArrayOfString();
                        changedList?.Add("AssistanceDataServers");
                        break;
                    case "GpsRefreshRate":
                        reader.ReadSignature("u");
                        props.GpsRefreshRate = reader.ReadUInt32();
                        changedList?.Add("GpsRefreshRate");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SignalProperties
    {
        public uint Rate { get; set; } = default!;
        public uint RssiThreshold { get; set; } = default!;
        public bool ErrorRateThreshold { get; set; } = default!;
        public Dictionary<string, VariantValue> Cdma { get; set; } = default!;
        public Dictionary<string, VariantValue> Evdo { get; set; } = default!;
        public Dictionary<string, VariantValue> Gsm { get; set; } = default!;
        public Dictionary<string, VariantValue> Umts { get; set; } = default!;
        public Dictionary<string, VariantValue> Lte { get; set; } = default!;
        public Dictionary<string, VariantValue> Nr5g { get; set; } = default!;
    }
    partial class Signal : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Signal";
        public Signal(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task SetupAsync(uint rate)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "Setup");
                writer.WriteUInt32(rate);
                return writer.CreateMessage();
            }
        }
        public Task SetupThresholdsAsync(Dictionary<string, VariantValue> settings)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "SetupThresholds");
                writer.WriteDictionary(settings);
                return writer.CreateMessage();
            }
        }
        public Task SetRateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Rate");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetRssiThresholdAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("RssiThreshold");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetErrorRateThresholdAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("ErrorRateThreshold");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetCdmaAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Cdma");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEvdoAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Evdo");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetGsmAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Gsm");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetUmtsAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Umts");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetLteAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Lte");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetNr5gAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Nr5g");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task<uint> GetRateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Rate"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetRssiThresholdAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RssiThreshold"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<bool> GetErrorRateThresholdAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ErrorRateThreshold"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetCdmaAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Cdma"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetEvdoAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Evdo"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetGsmAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Gsm"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetUmtsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Umts"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetLteAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Lte"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetNr5gAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Nr5g"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<SignalProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static SignalProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SignalProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SignalProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SignalProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Rate": invalidated.Add("Rate"); break;
                        case "RssiThreshold": invalidated.Add("RssiThreshold"); break;
                        case "ErrorRateThreshold": invalidated.Add("ErrorRateThreshold"); break;
                        case "Cdma": invalidated.Add("Cdma"); break;
                        case "Evdo": invalidated.Add("Evdo"); break;
                        case "Gsm": invalidated.Add("Gsm"); break;
                        case "Umts": invalidated.Add("Umts"); break;
                        case "Lte": invalidated.Add("Lte"); break;
                        case "Nr5g": invalidated.Add("Nr5g"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SignalProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SignalProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Rate":
                        reader.ReadSignature("u");
                        props.Rate = reader.ReadUInt32();
                        changedList?.Add("Rate");
                        break;
                    case "RssiThreshold":
                        reader.ReadSignature("u");
                        props.RssiThreshold = reader.ReadUInt32();
                        changedList?.Add("RssiThreshold");
                        break;
                    case "ErrorRateThreshold":
                        reader.ReadSignature("b");
                        props.ErrorRateThreshold = reader.ReadBool();
                        changedList?.Add("ErrorRateThreshold");
                        break;
                    case "Cdma":
                        reader.ReadSignature("a{sv}");
                        props.Cdma = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Cdma");
                        break;
                    case "Evdo":
                        reader.ReadSignature("a{sv}");
                        props.Evdo = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Evdo");
                        break;
                    case "Gsm":
                        reader.ReadSignature("a{sv}");
                        props.Gsm = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Gsm");
                        break;
                    case "Umts":
                        reader.ReadSignature("a{sv}");
                        props.Umts = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Umts");
                        break;
                    case "Lte":
                        reader.ReadSignature("a{sv}");
                        props.Lte = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Lte");
                        break;
                    case "Nr5g":
                        reader.ReadSignature("a{sv}");
                        props.Nr5g = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Nr5g");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record UssdProperties
    {
        public uint State { get; set; } = default!;
        public string NetworkNotification { get; set; } = default!;
        public string NetworkRequest { get; set; } = default!;
    }
    partial class Ussd : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Modem3gpp.Ussd";
        public Ussd(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<string> InitiateAsync(string command)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_s(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "Initiate");
                writer.WriteString(command);
                return writer.CreateMessage();
            }
        }
        public Task<string> RespondAsync(string response)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_s(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "Respond");
                writer.WriteString(response);
                return writer.CreateMessage();
            }
        }
        public Task CancelAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Cancel");
                return writer.CreateMessage();
            }
        }
        public Task SetStateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("State");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetNetworkNotificationAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("NetworkNotification");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetNetworkRequestAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("NetworkRequest");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task<uint> GetStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<string> GetNetworkNotificationAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NetworkNotification"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetNetworkRequestAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NetworkRequest"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<UssdProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static UssdProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<UssdProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<UssdProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<UssdProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "State": invalidated.Add("State"); break;
                        case "NetworkNotification": invalidated.Add("NetworkNotification"); break;
                        case "NetworkRequest": invalidated.Add("NetworkRequest"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static UssdProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new UssdProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "State":
                        reader.ReadSignature("u");
                        props.State = reader.ReadUInt32();
                        changedList?.Add("State");
                        break;
                    case "NetworkNotification":
                        reader.ReadSignature("s");
                        props.NetworkNotification = reader.ReadString();
                        changedList?.Add("NetworkNotification");
                        break;
                    case "NetworkRequest":
                        reader.ReadSignature("s");
                        props.NetworkRequest = reader.ReadString();
                        changedList?.Add("NetworkRequest");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record MessagingProperties
    {
        public ObjectPath[] Messages { get; set; } = default!;
        public uint[] SupportedStorages { get; set; } = default!;
        public uint DefaultStorage { get; set; } = default!;
    }
    partial class Messaging : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Messaging";
        public Messaging(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<ObjectPath[]> ListAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_ao(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "List");
                return writer.CreateMessage();
            }
        }
        public Task DeleteAsync(ObjectPath path)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "o",
                    member: "Delete");
                writer.WriteObjectPath(path);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> CreateAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "Create");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchAddedAsync(Action<Exception?, (ObjectPath Path, bool Received)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "Added", (Message m, object? s) => ReadMessage_ob(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchDeletedAsync(Action<Exception?, ObjectPath> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "Deleted", (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public Task SetMessagesAsync(ObjectPath[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Messages");
                writer.WriteSignature("ao");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedStoragesAsync(uint[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedStorages");
                writer.WriteSignature("au");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDefaultStorageAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("DefaultStorage");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath[]> GetMessagesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Messages"), (Message m, object? s) => ReadMessage_v_ao(m, (ModemManager1Object)s!), this);
        public Task<uint[]> GetSupportedStoragesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedStorages"), (Message m, object? s) => ReadMessage_v_au(m, (ModemManager1Object)s!), this);
        public Task<uint> GetDefaultStorageAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DefaultStorage"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<MessagingProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static MessagingProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<MessagingProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<MessagingProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<MessagingProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Messages": invalidated.Add("Messages"); break;
                        case "SupportedStorages": invalidated.Add("SupportedStorages"); break;
                        case "DefaultStorage": invalidated.Add("DefaultStorage"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static MessagingProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new MessagingProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Messages":
                        reader.ReadSignature("ao");
                        props.Messages = reader.ReadArrayOfObjectPath();
                        changedList?.Add("Messages");
                        break;
                    case "SupportedStorages":
                        reader.ReadSignature("au");
                        props.SupportedStorages = reader.ReadArrayOfUInt32();
                        changedList?.Add("SupportedStorages");
                        break;
                    case "DefaultStorage":
                        reader.ReadSignature("u");
                        props.DefaultStorage = reader.ReadUInt32();
                        changedList?.Add("DefaultStorage");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record ModemProperties
    {
        public ObjectPath Sim { get; set; } = default!;
        public ObjectPath[] SimSlots { get; set; } = default!;
        public uint PrimarySimSlot { get; set; } = default!;
        public ObjectPath[] Bearers { get; set; } = default!;
        public uint[] SupportedCapabilities { get; set; } = default!;
        public uint CurrentCapabilities { get; set; } = default!;
        public uint MaxBearers { get; set; } = default!;
        public uint MaxActiveBearers { get; set; } = default!;
        public uint MaxActiveMultiplexedBearers { get; set; } = default!;
        public string Manufacturer { get; set; } = default!;
        public string Model { get; set; } = default!;
        public string Revision { get; set; } = default!;
        public string CarrierConfiguration { get; set; } = default!;
        public string CarrierConfigurationRevision { get; set; } = default!;
        public string HardwareRevision { get; set; } = default!;
        public string DeviceIdentifier { get; set; } = default!;
        public string Device { get; set; } = default!;
        public string[] Drivers { get; set; } = default!;
        public string Plugin { get; set; } = default!;
        public string PrimaryPort { get; set; } = default!;
        public (string, uint)[] Ports { get; set; } = default!;
        public string EquipmentIdentifier { get; set; } = default!;
        public uint UnlockRequired { get; set; } = default!;
        public Dictionary<uint, uint> UnlockRetries { get; set; } = default!;
        public int State { get; set; } = default!;
        public uint StateFailedReason { get; set; } = default!;
        public uint AccessTechnologies { get; set; } = default!;
        public (uint, bool) SignalQuality { get; set; } = default!;
        public string[] OwnNumbers { get; set; } = default!;
        public uint PowerState { get; set; } = default!;
        public (uint, uint)[] SupportedModes { get; set; } = default!;
        public (uint, uint) CurrentModes { get; set; } = default!;
        public uint[] SupportedBands { get; set; } = default!;
        public uint[] CurrentBands { get; set; } = default!;
        public uint SupportedIpFamilies { get; set; } = default!;
    }
    partial class Modem : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem";
        public Modem(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task EnableAsync(bool enable)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Enable");
                writer.WriteBool(enable);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath[]> ListBearersAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_ao(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListBearers");
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> CreateBearerAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "CreateBearer");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public Task DeleteBearerAsync(ObjectPath bearer)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "o",
                    member: "DeleteBearer");
                writer.WriteObjectPath(bearer);
                return writer.CreateMessage();
            }
        }
        public Task ResetAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Reset");
                return writer.CreateMessage();
            }
        }
        public Task FactoryResetAsync(string code)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "FactoryReset");
                writer.WriteString(code);
                return writer.CreateMessage();
            }
        }
        public Task SetPowerStateAsyncMethod1(uint state)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetPowerState");
                writer.WriteUInt32(state);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentCapabilitiesAsyncMethod1(uint capabilities)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetCurrentCapabilities");
                writer.WriteUInt32(capabilities);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentModesAsyncMethod1((uint, uint) modes)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "(uu)",
                    member: "SetCurrentModes");
                WriteType_ruuz(ref writer, modes);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentBandsAsyncMethod1(uint[] bands)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "au",
                    member: "SetCurrentBands");
                writer.WriteArray(bands);
                return writer.CreateMessage();
            }
        }
        public Task SetPrimarySimSlotAsyncMethod1(uint simSlot)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetPrimarySimSlot");
                writer.WriteUInt32(simSlot);
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<string, VariantValue>[]> GetCellInfoAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aaesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "GetCellInfo");
                return writer.CreateMessage();
            }
        }
        public Task<string> CommandAsync(string cmd, uint timeout)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_s(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "su",
                    member: "Command");
                writer.WriteString(cmd);
                writer.WriteUInt32(timeout);
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchStateChangedAsync(Action<Exception?, (int Old, int New, uint Reason)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "StateChanged", (Message m, object? s) => ReadMessage_iiu(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public Task SetSimAsync(ObjectPath value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Sim");
                writer.WriteSignature("o");
                writer.WriteObjectPath(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSimSlotsAsync(ObjectPath[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SimSlots");
                writer.WriteSignature("ao");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPrimarySimSlotAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PrimarySimSlot");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetBearersAsync(ObjectPath[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Bearers");
                writer.WriteSignature("ao");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedCapabilitiesAsync(uint[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedCapabilities");
                writer.WriteSignature("au");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentCapabilitiesAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("CurrentCapabilities");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetMaxBearersAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("MaxBearers");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetMaxActiveBearersAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("MaxActiveBearers");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetMaxActiveMultiplexedBearersAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("MaxActiveMultiplexedBearers");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetManufacturerAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Manufacturer");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetModelAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Model");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetRevisionAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Revision");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetCarrierConfigurationAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("CarrierConfiguration");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetCarrierConfigurationRevisionAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("CarrierConfigurationRevision");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetHardwareRevisionAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("HardwareRevision");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDeviceIdentifierAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("DeviceIdentifier");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDeviceAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Device");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDriversAsync(string[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Drivers");
                writer.WriteSignature("as");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPluginAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Plugin");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPrimaryPortAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PrimaryPort");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPortsAsync((string, uint)[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Ports");
                writer.WriteSignature("a(su)");
                WriteType_arsuz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetEquipmentIdentifierAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EquipmentIdentifier");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetUnlockRequiredAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("UnlockRequired");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetUnlockRetriesAsync(Dictionary<uint, uint> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("UnlockRetries");
                writer.WriteSignature("a{uu}");
                WriteType_aeuu(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetStateAsync(int value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("State");
                writer.WriteSignature("i");
                writer.WriteInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetStateFailedReasonAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("StateFailedReason");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetAccessTechnologiesAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("AccessTechnologies");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSignalQualityAsync((uint, bool) value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SignalQuality");
                writer.WriteSignature("(ub)");
                WriteType_rubz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetOwnNumbersAsync(string[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("OwnNumbers");
                writer.WriteSignature("as");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPowerStateAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PowerState");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedModesAsync((uint, uint)[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedModes");
                writer.WriteSignature("a(uu)");
                WriteType_aruuz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentModesAsyncMethod2((uint, uint) value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("CurrentModes");
                writer.WriteSignature("(uu)");
                WriteType_ruuz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedBandsAsync(uint[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedBands");
                writer.WriteSignature("au");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetCurrentBandsAsyncMethod2(uint[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("CurrentBands");
                writer.WriteSignature("au");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSupportedIpFamiliesAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SupportedIpFamilies");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> GetSimAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Sim"), (Message m, object? s) => ReadMessage_v_o(m, (ModemManager1Object)s!), this);
        public Task<ObjectPath[]> GetSimSlotsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SimSlots"), (Message m, object? s) => ReadMessage_v_ao(m, (ModemManager1Object)s!), this);
        public Task<uint> GetPrimarySimSlotAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PrimarySimSlot"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<ObjectPath[]> GetBearersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Bearers"), (Message m, object? s) => ReadMessage_v_ao(m, (ModemManager1Object)s!), this);
        public Task<uint[]> GetSupportedCapabilitiesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedCapabilities"), (Message m, object? s) => ReadMessage_v_au(m, (ModemManager1Object)s!), this);
        public Task<uint> GetCurrentCapabilitiesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CurrentCapabilities"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetMaxBearersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "MaxBearers"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetMaxActiveBearersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "MaxActiveBearers"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetMaxActiveMultiplexedBearersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "MaxActiveMultiplexedBearers"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<string> GetManufacturerAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Manufacturer"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetModelAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Model"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetRevisionAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Revision"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetCarrierConfigurationAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CarrierConfiguration"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetCarrierConfigurationRevisionAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CarrierConfigurationRevision"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetHardwareRevisionAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HardwareRevision"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetDeviceIdentifierAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DeviceIdentifier"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetDeviceAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Device"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string[]> GetDriversAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Drivers"), (Message m, object? s) => ReadMessage_v_as(m, (ModemManager1Object)s!), this);
        public Task<string> GetPluginAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Plugin"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetPrimaryPortAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PrimaryPort"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<(string, uint)[]> GetPortsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Ports"), (Message m, object? s) => ReadMessage_v_arsuz(m, (ModemManager1Object)s!), this);
        public Task<string> GetEquipmentIdentifierAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EquipmentIdentifier"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<uint> GetUnlockRequiredAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "UnlockRequired"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<uint, uint>> GetUnlockRetriesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "UnlockRetries"), (Message m, object? s) => ReadMessage_v_aeuu(m, (ModemManager1Object)s!), this);
        public Task<int> GetStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (Message m, object? s) => ReadMessage_v_i(m, (ModemManager1Object)s!), this);
        public Task<uint> GetStateFailedReasonAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "StateFailedReason"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetAccessTechnologiesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "AccessTechnologies"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<(uint, bool)> GetSignalQualityAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SignalQuality"), (Message m, object? s) => ReadMessage_v_rubz(m, (ModemManager1Object)s!), this);
        public Task<string[]> GetOwnNumbersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OwnNumbers"), (Message m, object? s) => ReadMessage_v_as(m, (ModemManager1Object)s!), this);
        public Task<uint> GetPowerStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PowerState"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<(uint, uint)[]> GetSupportedModesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedModes"), (Message m, object? s) => ReadMessage_v_aruuz(m, (ModemManager1Object)s!), this);
        public Task<(uint, uint)> GetCurrentModesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CurrentModes"), (Message m, object? s) => ReadMessage_v_ruuz(m, (ModemManager1Object)s!), this);
        public Task<uint[]> GetSupportedBandsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedBands"), (Message m, object? s) => ReadMessage_v_au(m, (ModemManager1Object)s!), this);
        public Task<uint[]> GetCurrentBandsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CurrentBands"), (Message m, object? s) => ReadMessage_v_au(m, (ModemManager1Object)s!), this);
        public Task<uint> GetSupportedIpFamiliesAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SupportedIpFamilies"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<ModemProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static ModemProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<ModemProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<ModemProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<ModemProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Sim": invalidated.Add("Sim"); break;
                        case "SimSlots": invalidated.Add("SimSlots"); break;
                        case "PrimarySimSlot": invalidated.Add("PrimarySimSlot"); break;
                        case "Bearers": invalidated.Add("Bearers"); break;
                        case "SupportedCapabilities": invalidated.Add("SupportedCapabilities"); break;
                        case "CurrentCapabilities": invalidated.Add("CurrentCapabilities"); break;
                        case "MaxBearers": invalidated.Add("MaxBearers"); break;
                        case "MaxActiveBearers": invalidated.Add("MaxActiveBearers"); break;
                        case "MaxActiveMultiplexedBearers": invalidated.Add("MaxActiveMultiplexedBearers"); break;
                        case "Manufacturer": invalidated.Add("Manufacturer"); break;
                        case "Model": invalidated.Add("Model"); break;
                        case "Revision": invalidated.Add("Revision"); break;
                        case "CarrierConfiguration": invalidated.Add("CarrierConfiguration"); break;
                        case "CarrierConfigurationRevision": invalidated.Add("CarrierConfigurationRevision"); break;
                        case "HardwareRevision": invalidated.Add("HardwareRevision"); break;
                        case "DeviceIdentifier": invalidated.Add("DeviceIdentifier"); break;
                        case "Device": invalidated.Add("Device"); break;
                        case "Drivers": invalidated.Add("Drivers"); break;
                        case "Plugin": invalidated.Add("Plugin"); break;
                        case "PrimaryPort": invalidated.Add("PrimaryPort"); break;
                        case "Ports": invalidated.Add("Ports"); break;
                        case "EquipmentIdentifier": invalidated.Add("EquipmentIdentifier"); break;
                        case "UnlockRequired": invalidated.Add("UnlockRequired"); break;
                        case "UnlockRetries": invalidated.Add("UnlockRetries"); break;
                        case "State": invalidated.Add("State"); break;
                        case "StateFailedReason": invalidated.Add("StateFailedReason"); break;
                        case "AccessTechnologies": invalidated.Add("AccessTechnologies"); break;
                        case "SignalQuality": invalidated.Add("SignalQuality"); break;
                        case "OwnNumbers": invalidated.Add("OwnNumbers"); break;
                        case "PowerState": invalidated.Add("PowerState"); break;
                        case "SupportedModes": invalidated.Add("SupportedModes"); break;
                        case "CurrentModes": invalidated.Add("CurrentModes"); break;
                        case "SupportedBands": invalidated.Add("SupportedBands"); break;
                        case "CurrentBands": invalidated.Add("CurrentBands"); break;
                        case "SupportedIpFamilies": invalidated.Add("SupportedIpFamilies"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static ModemProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new ModemProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Sim":
                        reader.ReadSignature("o");
                        props.Sim = reader.ReadObjectPath();
                        changedList?.Add("Sim");
                        break;
                    case "SimSlots":
                        reader.ReadSignature("ao");
                        props.SimSlots = reader.ReadArrayOfObjectPath();
                        changedList?.Add("SimSlots");
                        break;
                    case "PrimarySimSlot":
                        reader.ReadSignature("u");
                        props.PrimarySimSlot = reader.ReadUInt32();
                        changedList?.Add("PrimarySimSlot");
                        break;
                    case "Bearers":
                        reader.ReadSignature("ao");
                        props.Bearers = reader.ReadArrayOfObjectPath();
                        changedList?.Add("Bearers");
                        break;
                    case "SupportedCapabilities":
                        reader.ReadSignature("au");
                        props.SupportedCapabilities = reader.ReadArrayOfUInt32();
                        changedList?.Add("SupportedCapabilities");
                        break;
                    case "CurrentCapabilities":
                        reader.ReadSignature("u");
                        props.CurrentCapabilities = reader.ReadUInt32();
                        changedList?.Add("CurrentCapabilities");
                        break;
                    case "MaxBearers":
                        reader.ReadSignature("u");
                        props.MaxBearers = reader.ReadUInt32();
                        changedList?.Add("MaxBearers");
                        break;
                    case "MaxActiveBearers":
                        reader.ReadSignature("u");
                        props.MaxActiveBearers = reader.ReadUInt32();
                        changedList?.Add("MaxActiveBearers");
                        break;
                    case "MaxActiveMultiplexedBearers":
                        reader.ReadSignature("u");
                        props.MaxActiveMultiplexedBearers = reader.ReadUInt32();
                        changedList?.Add("MaxActiveMultiplexedBearers");
                        break;
                    case "Manufacturer":
                        reader.ReadSignature("s");
                        props.Manufacturer = reader.ReadString();
                        changedList?.Add("Manufacturer");
                        break;
                    case "Model":
                        reader.ReadSignature("s");
                        props.Model = reader.ReadString();
                        changedList?.Add("Model");
                        break;
                    case "Revision":
                        reader.ReadSignature("s");
                        props.Revision = reader.ReadString();
                        changedList?.Add("Revision");
                        break;
                    case "CarrierConfiguration":
                        reader.ReadSignature("s");
                        props.CarrierConfiguration = reader.ReadString();
                        changedList?.Add("CarrierConfiguration");
                        break;
                    case "CarrierConfigurationRevision":
                        reader.ReadSignature("s");
                        props.CarrierConfigurationRevision = reader.ReadString();
                        changedList?.Add("CarrierConfigurationRevision");
                        break;
                    case "HardwareRevision":
                        reader.ReadSignature("s");
                        props.HardwareRevision = reader.ReadString();
                        changedList?.Add("HardwareRevision");
                        break;
                    case "DeviceIdentifier":
                        reader.ReadSignature("s");
                        props.DeviceIdentifier = reader.ReadString();
                        changedList?.Add("DeviceIdentifier");
                        break;
                    case "Device":
                        reader.ReadSignature("s");
                        props.Device = reader.ReadString();
                        changedList?.Add("Device");
                        break;
                    case "Drivers":
                        reader.ReadSignature("as");
                        props.Drivers = reader.ReadArrayOfString();
                        changedList?.Add("Drivers");
                        break;
                    case "Plugin":
                        reader.ReadSignature("s");
                        props.Plugin = reader.ReadString();
                        changedList?.Add("Plugin");
                        break;
                    case "PrimaryPort":
                        reader.ReadSignature("s");
                        props.PrimaryPort = reader.ReadString();
                        changedList?.Add("PrimaryPort");
                        break;
                    case "Ports":
                        reader.ReadSignature("a(su)");
                        props.Ports = ReadType_arsuz(ref reader);
                        changedList?.Add("Ports");
                        break;
                    case "EquipmentIdentifier":
                        reader.ReadSignature("s");
                        props.EquipmentIdentifier = reader.ReadString();
                        changedList?.Add("EquipmentIdentifier");
                        break;
                    case "UnlockRequired":
                        reader.ReadSignature("u");
                        props.UnlockRequired = reader.ReadUInt32();
                        changedList?.Add("UnlockRequired");
                        break;
                    case "UnlockRetries":
                        reader.ReadSignature("a{uu}");
                        props.UnlockRetries = ReadType_aeuu(ref reader);
                        changedList?.Add("UnlockRetries");
                        break;
                    case "State":
                        reader.ReadSignature("i");
                        props.State = reader.ReadInt32();
                        changedList?.Add("State");
                        break;
                    case "StateFailedReason":
                        reader.ReadSignature("u");
                        props.StateFailedReason = reader.ReadUInt32();
                        changedList?.Add("StateFailedReason");
                        break;
                    case "AccessTechnologies":
                        reader.ReadSignature("u");
                        props.AccessTechnologies = reader.ReadUInt32();
                        changedList?.Add("AccessTechnologies");
                        break;
                    case "SignalQuality":
                        reader.ReadSignature("(ub)");
                        props.SignalQuality = ReadType_rubz(ref reader);
                        changedList?.Add("SignalQuality");
                        break;
                    case "OwnNumbers":
                        reader.ReadSignature("as");
                        props.OwnNumbers = reader.ReadArrayOfString();
                        changedList?.Add("OwnNumbers");
                        break;
                    case "PowerState":
                        reader.ReadSignature("u");
                        props.PowerState = reader.ReadUInt32();
                        changedList?.Add("PowerState");
                        break;
                    case "SupportedModes":
                        reader.ReadSignature("a(uu)");
                        props.SupportedModes = ReadType_aruuz(ref reader);
                        changedList?.Add("SupportedModes");
                        break;
                    case "CurrentModes":
                        reader.ReadSignature("(uu)");
                        props.CurrentModes = ReadType_ruuz(ref reader);
                        changedList?.Add("CurrentModes");
                        break;
                    case "SupportedBands":
                        reader.ReadSignature("au");
                        props.SupportedBands = reader.ReadArrayOfUInt32();
                        changedList?.Add("SupportedBands");
                        break;
                    case "CurrentBands":
                        reader.ReadSignature("au");
                        props.CurrentBands = reader.ReadArrayOfUInt32();
                        changedList?.Add("CurrentBands");
                        break;
                    case "SupportedIpFamilies":
                        reader.ReadSignature("u");
                        props.SupportedIpFamilies = reader.ReadUInt32();
                        changedList?.Add("SupportedIpFamilies");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record TimeProperties
    {
        public Dictionary<string, VariantValue> NetworkTimezone { get; set; } = default!;
    }
    partial class Time : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Time";
        public Time(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<string> GetNetworkTimeAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_s(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "GetNetworkTime");
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchNetworkTimeChangedAsync(Action<Exception?, string> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "NetworkTimeChanged", (Message m, object? s) => ReadMessage_s(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public Task SetNetworkTimezoneAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("NetworkTimezone");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<string, VariantValue>> GetNetworkTimezoneAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NetworkTimezone"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<TimeProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static TimeProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<TimeProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<TimeProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<TimeProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "NetworkTimezone": invalidated.Add("NetworkTimezone"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static TimeProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new TimeProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "NetworkTimezone":
                        reader.ReadSignature("a{sv}");
                        props.NetworkTimezone = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("NetworkTimezone");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record FirmwareProperties
    {
        public (uint, Dictionary<string, VariantValue>) UpdateSettings { get; set; } = default!;
    }
    partial class Firmware : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Firmware";
        public Firmware(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<(string Selected, Dictionary<string, VariantValue>[] Installed)> ListAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_saaesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "List");
                return writer.CreateMessage();
            }
        }
        public Task SelectAsync(string uniqueid)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "Select");
                writer.WriteString(uniqueid);
                return writer.CreateMessage();
            }
        }
        public Task SetUpdateSettingsAsync((uint, Dictionary<string, VariantValue>) value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("UpdateSettings");
                writer.WriteSignature("(ua{sv})");
                WriteType_ruaesvz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task<(uint, Dictionary<string, VariantValue>)> GetUpdateSettingsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "UpdateSettings"), (Message m, object? s) => ReadMessage_v_ruaesvz(m, (ModemManager1Object)s!), this);
        public Task<FirmwareProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static FirmwareProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<FirmwareProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<FirmwareProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<FirmwareProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "UpdateSettings": invalidated.Add("UpdateSettings"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static FirmwareProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new FirmwareProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "UpdateSettings":
                        reader.ReadSignature("(ua{sv})");
                        props.UpdateSettings = ReadType_ruaesvz(ref reader);
                        changedList?.Add("UpdateSettings");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record ProfileManagerProperties
    {
        public string IndexField { get; set; } = default!;
    }
    partial class ProfileManager : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Modem3gpp.ProfileManager";
        public ProfileManager(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<Dictionary<string, VariantValue>[]> ListAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aaesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "List");
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<string, VariantValue>> SetAsync(Dictionary<string, VariantValue> requestedProperties)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "Set");
                writer.WriteDictionary(requestedProperties);
                return writer.CreateMessage();
            }
        }
        public Task DeleteAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "Delete");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchUpdatedAsync(Action<Exception?> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "Updated", handler, emitOnCapturedContext, flags);
        public Task SetIndexFieldAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("IndexField");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task<string> GetIndexFieldAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IndexField"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<ProfileManagerProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static ProfileManagerProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<ProfileManagerProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<ProfileManagerProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<ProfileManagerProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "IndexField": invalidated.Add("IndexField"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static ProfileManagerProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new ProfileManagerProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "IndexField":
                        reader.ReadSignature("s");
                        props.IndexField = reader.ReadString();
                        changedList?.Add("IndexField");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SarProperties
    {
        public bool State { get; set; } = default!;
        public uint PowerLevel { get; set; } = default!;
    }
    partial class Sar : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Sar";
        public Sar(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task EnableAsync(bool enable)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Enable");
                writer.WriteBool(enable);
                return writer.CreateMessage();
            }
        }
        public Task SetPowerLevelAsyncMethod1(uint level)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetPowerLevel");
                writer.WriteUInt32(level);
                return writer.CreateMessage();
            }
        }
        public Task SetStateAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("State");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPowerLevelAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PowerLevel");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<bool> GetStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<uint> GetPowerLevelAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PowerLevel"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<SarProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static SarProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SarProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SarProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SarProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "State": invalidated.Add("State"); break;
                        case "PowerLevel": invalidated.Add("PowerLevel"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SarProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SarProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "State":
                        reader.ReadSignature("b");
                        props.State = reader.ReadBool();
                        changedList?.Add("State");
                        break;
                    case "PowerLevel":
                        reader.ReadSignature("u");
                        props.PowerLevel = reader.ReadUInt32();
                        changedList?.Add("PowerLevel");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    partial class Simple : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Simple";
        public Simple(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<ObjectPath> ConnectAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "Connect");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public Task DisconnectAsync(ObjectPath bearer)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "o",
                    member: "Disconnect");
                writer.WriteObjectPath(bearer);
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<string, VariantValue>> GetStatusAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "GetStatus");
                return writer.CreateMessage();
            }
        }
    }
    record Modem3gppProperties
    {
        public string Imei { get; set; } = default!;
        public uint RegistrationState { get; set; } = default!;
        public string OperatorCode { get; set; } = default!;
        public string OperatorName { get; set; } = default!;
        public uint EnabledFacilityLocks { get; set; } = default!;
        public uint SubscriptionState { get; set; } = default!;
        public uint EpsUeModeOperation { get; set; } = default!;
        public (uint, bool, byte[])[] Pco { get; set; } = default!;
        public ObjectPath InitialEpsBearer { get; set; } = default!;
        public Dictionary<string, VariantValue> InitialEpsBearerSettings { get; set; } = default!;
        public uint PacketServiceState { get; set; } = default!;
        public Dictionary<string, VariantValue> Nr5gRegistrationSettings { get; set; } = default!;
    }
    partial class Modem3gpp : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Modem3gpp";
        public Modem3gpp(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task RegisterAsync(string operatorId)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "Register");
                writer.WriteString(operatorId);
                return writer.CreateMessage();
            }
        }
        public Task<Dictionary<string, VariantValue>[]> ScanAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_aaesv(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Scan");
                return writer.CreateMessage();
            }
        }
        public Task SetEpsUeModeOperationAsyncMethod1(uint mode)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetEpsUeModeOperation");
                writer.WriteUInt32(mode);
                return writer.CreateMessage();
            }
        }
        public Task SetInitialEpsBearerSettingsAsyncMethod1(Dictionary<string, VariantValue> settings)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "SetInitialEpsBearerSettings");
                writer.WriteDictionary(settings);
                return writer.CreateMessage();
            }
        }
        public Task SetNr5gRegistrationSettingsAsyncMethod1(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "SetNr5gRegistrationSettings");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public Task DisableFacilityLockAsync((uint, string) properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "(us)",
                    member: "DisableFacilityLock");
                WriteType_rusz(ref writer, properties);
                return writer.CreateMessage();
            }
        }
        public Task SetPacketServiceStateAsyncMethod1(uint state)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SetPacketServiceState");
                writer.WriteUInt32(state);
                return writer.CreateMessage();
            }
        }
        public Task SetImeiAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Imei");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetRegistrationStateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("RegistrationState");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetOperatorCodeAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("OperatorCode");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetOperatorNameAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("OperatorName");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEnabledFacilityLocksAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EnabledFacilityLocks");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSubscriptionStateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SubscriptionState");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEpsUeModeOperationAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EpsUeModeOperation");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPcoAsync((uint, bool, byte[])[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Pco");
                writer.WriteSignature("a(ubay)");
                WriteType_arubayz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetInitialEpsBearerAsync(ObjectPath value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("InitialEpsBearer");
                writer.WriteSignature("o");
                writer.WriteObjectPath(value);
                return writer.CreateMessage();
            }
        }
        public Task SetInitialEpsBearerSettingsAsyncMethod2(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("InitialEpsBearerSettings");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPacketServiceStateAsyncMethod2(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PacketServiceState");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetNr5gRegistrationSettingsAsyncMethod2(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Nr5gRegistrationSettings");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task<string> GetImeiAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Imei"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<uint> GetRegistrationStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RegistrationState"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<string> GetOperatorCodeAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OperatorCode"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetOperatorNameAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OperatorName"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<uint> GetEnabledFacilityLocksAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EnabledFacilityLocks"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetSubscriptionStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SubscriptionState"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetEpsUeModeOperationAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EpsUeModeOperation"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<(uint, bool, byte[])[]> GetPcoAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Pco"), (Message m, object? s) => ReadMessage_v_arubayz(m, (ModemManager1Object)s!), this);
        public Task<ObjectPath> GetInitialEpsBearerAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "InitialEpsBearer"), (Message m, object? s) => ReadMessage_v_o(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetInitialEpsBearerSettingsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "InitialEpsBearerSettings"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<uint> GetPacketServiceStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PacketServiceState"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetNr5gRegistrationSettingsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Nr5gRegistrationSettings"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Modem3gppProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static Modem3gppProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<Modem3gppProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<Modem3gppProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<Modem3gppProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Imei": invalidated.Add("Imei"); break;
                        case "RegistrationState": invalidated.Add("RegistrationState"); break;
                        case "OperatorCode": invalidated.Add("OperatorCode"); break;
                        case "OperatorName": invalidated.Add("OperatorName"); break;
                        case "EnabledFacilityLocks": invalidated.Add("EnabledFacilityLocks"); break;
                        case "SubscriptionState": invalidated.Add("SubscriptionState"); break;
                        case "EpsUeModeOperation": invalidated.Add("EpsUeModeOperation"); break;
                        case "Pco": invalidated.Add("Pco"); break;
                        case "InitialEpsBearer": invalidated.Add("InitialEpsBearer"); break;
                        case "InitialEpsBearerSettings": invalidated.Add("InitialEpsBearerSettings"); break;
                        case "PacketServiceState": invalidated.Add("PacketServiceState"); break;
                        case "Nr5gRegistrationSettings": invalidated.Add("Nr5gRegistrationSettings"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static Modem3gppProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new Modem3gppProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Imei":
                        reader.ReadSignature("s");
                        props.Imei = reader.ReadString();
                        changedList?.Add("Imei");
                        break;
                    case "RegistrationState":
                        reader.ReadSignature("u");
                        props.RegistrationState = reader.ReadUInt32();
                        changedList?.Add("RegistrationState");
                        break;
                    case "OperatorCode":
                        reader.ReadSignature("s");
                        props.OperatorCode = reader.ReadString();
                        changedList?.Add("OperatorCode");
                        break;
                    case "OperatorName":
                        reader.ReadSignature("s");
                        props.OperatorName = reader.ReadString();
                        changedList?.Add("OperatorName");
                        break;
                    case "EnabledFacilityLocks":
                        reader.ReadSignature("u");
                        props.EnabledFacilityLocks = reader.ReadUInt32();
                        changedList?.Add("EnabledFacilityLocks");
                        break;
                    case "SubscriptionState":
                        reader.ReadSignature("u");
                        props.SubscriptionState = reader.ReadUInt32();
                        changedList?.Add("SubscriptionState");
                        break;
                    case "EpsUeModeOperation":
                        reader.ReadSignature("u");
                        props.EpsUeModeOperation = reader.ReadUInt32();
                        changedList?.Add("EpsUeModeOperation");
                        break;
                    case "Pco":
                        reader.ReadSignature("a(ubay)");
                        props.Pco = ReadType_arubayz(ref reader);
                        changedList?.Add("Pco");
                        break;
                    case "InitialEpsBearer":
                        reader.ReadSignature("o");
                        props.InitialEpsBearer = reader.ReadObjectPath();
                        changedList?.Add("InitialEpsBearer");
                        break;
                    case "InitialEpsBearerSettings":
                        reader.ReadSignature("a{sv}");
                        props.InitialEpsBearerSettings = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("InitialEpsBearerSettings");
                        break;
                    case "PacketServiceState":
                        reader.ReadSignature("u");
                        props.PacketServiceState = reader.ReadUInt32();
                        changedList?.Add("PacketServiceState");
                        break;
                    case "Nr5gRegistrationSettings":
                        reader.ReadSignature("a{sv}");
                        props.Nr5gRegistrationSettings = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Nr5gRegistrationSettings");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record VoiceProperties
    {
        public ObjectPath[] Calls { get; set; } = default!;
        public bool EmergencyOnly { get; set; } = default!;
    }
    partial class Voice : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Modem.Voice";
        public Voice(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<ObjectPath[]> ListCallsAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_ao(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListCalls");
                return writer.CreateMessage();
            }
        }
        public Task DeleteCallAsync(ObjectPath path)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "o",
                    member: "DeleteCall");
                writer.WriteObjectPath(path);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> CreateCallAsync(Dictionary<string, VariantValue> properties)
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a{sv}",
                    member: "CreateCall");
                writer.WriteDictionary(properties);
                return writer.CreateMessage();
            }
        }
        public Task HoldAndAcceptAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "HoldAndAccept");
                return writer.CreateMessage();
            }
        }
        public Task HangupAndAcceptAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "HangupAndAccept");
                return writer.CreateMessage();
            }
        }
        public Task HangupAllAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "HangupAll");
                return writer.CreateMessage();
            }
        }
        public Task TransferAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Transfer");
                return writer.CreateMessage();
            }
        }
        public Task CallWaitingSetupAsync(bool enable)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "CallWaitingSetup");
                writer.WriteBool(enable);
                return writer.CreateMessage();
            }
        }
        public Task<bool> CallWaitingQueryAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage(), (Message m, object? s) => ReadMessage_b(m, (ModemManager1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CallWaitingQuery");
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchCallAddedAsync(Action<Exception?, ObjectPath> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "CallAdded", (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchCallDeletedAsync(Action<Exception?, ObjectPath> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => base.WatchSignalAsync(Service.Destination, __Interface, Path, "CallDeleted", (Message m, object? s) => ReadMessage_o(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
        public Task SetCallsAsync(ObjectPath[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Calls");
                writer.WriteSignature("ao");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEmergencyOnlyAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EmergencyOnly");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath[]> GetCallsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Calls"), (Message m, object? s) => ReadMessage_v_ao(m, (ModemManager1Object)s!), this);
        public Task<bool> GetEmergencyOnlyAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EmergencyOnly"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<VoiceProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static VoiceProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<VoiceProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<VoiceProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<VoiceProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Calls": invalidated.Add("Calls"); break;
                        case "EmergencyOnly": invalidated.Add("EmergencyOnly"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static VoiceProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new VoiceProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Calls":
                        reader.ReadSignature("ao");
                        props.Calls = reader.ReadArrayOfObjectPath();
                        changedList?.Add("Calls");
                        break;
                    case "EmergencyOnly":
                        reader.ReadSignature("b");
                        props.EmergencyOnly = reader.ReadBool();
                        changedList?.Add("EmergencyOnly");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SimProperties
    {
        public bool Active { get; set; } = default!;
        public string SimIdentifier { get; set; } = default!;
        public string Imsi { get; set; } = default!;
        public string Eid { get; set; } = default!;
        public string OperatorIdentifier { get; set; } = default!;
        public string OperatorName { get; set; } = default!;
        public string[] EmergencyNumbers { get; set; } = default!;
        public (string, uint)[] PreferredNetworks { get; set; } = default!;
        public byte[] Gid1 { get; set; } = default!;
        public byte[] Gid2 { get; set; } = default!;
        public uint SimType { get; set; } = default!;
        public uint EsimStatus { get; set; } = default!;
        public uint Removability { get; set; } = default!;
    }
    partial class Sim : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Sim";
        public Sim(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task SendPinAsync(string pin)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SendPin");
                writer.WriteString(pin);
                return writer.CreateMessage();
            }
        }
        public Task SendPukAsync(string puk, string pin)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ss",
                    member: "SendPuk");
                writer.WriteString(puk);
                writer.WriteString(pin);
                return writer.CreateMessage();
            }
        }
        public Task EnablePinAsync(string pin, bool enabled)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "sb",
                    member: "EnablePin");
                writer.WriteString(pin);
                writer.WriteBool(enabled);
                return writer.CreateMessage();
            }
        }
        public Task ChangePinAsync(string oldPin, string newPin)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ss",
                    member: "ChangePin");
                writer.WriteString(oldPin);
                writer.WriteString(newPin);
                return writer.CreateMessage();
            }
        }
        public Task SetPreferredNetworksAsyncMethod1((string, uint)[] preferredNetworks)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "a(su)",
                    member: "SetPreferredNetworks");
                WriteType_arsuz(ref writer, preferredNetworks);
                return writer.CreateMessage();
            }
        }
        public Task SetActiveAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Active");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSimIdentifierAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SimIdentifier");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetImsiAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Imsi");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEidAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Eid");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetOperatorIdentifierAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("OperatorIdentifier");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetOperatorNameAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("OperatorName");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEmergencyNumbersAsync(string[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EmergencyNumbers");
                writer.WriteSignature("as");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPreferredNetworksAsyncMethod2((string, uint)[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PreferredNetworks");
                writer.WriteSignature("a(su)");
                WriteType_arsuz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetGid1Async(byte[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Gid1");
                writer.WriteSignature("ay");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetGid2Async(byte[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Gid2");
                writer.WriteSignature("ay");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSimTypeAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SimType");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetEsimStatusAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EsimStatus");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetRemovabilityAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Removability");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<bool> GetActiveAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Active"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<string> GetSimIdentifierAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SimIdentifier"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetImsiAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Imsi"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetEidAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Eid"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetOperatorIdentifierAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OperatorIdentifier"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetOperatorNameAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OperatorName"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string[]> GetEmergencyNumbersAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EmergencyNumbers"), (Message m, object? s) => ReadMessage_v_as(m, (ModemManager1Object)s!), this);
        public Task<(string, uint)[]> GetPreferredNetworksAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PreferredNetworks"), (Message m, object? s) => ReadMessage_v_arsuz(m, (ModemManager1Object)s!), this);
        public Task<byte[]> GetGid1Async()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Gid1"), (Message m, object? s) => ReadMessage_v_ay(m, (ModemManager1Object)s!), this);
        public Task<byte[]> GetGid2Async()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Gid2"), (Message m, object? s) => ReadMessage_v_ay(m, (ModemManager1Object)s!), this);
        public Task<uint> GetSimTypeAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SimType"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetEsimStatusAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EsimStatus"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetRemovabilityAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Removability"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<SimProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static SimProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SimProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SimProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SimProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Active": invalidated.Add("Active"); break;
                        case "SimIdentifier": invalidated.Add("SimIdentifier"); break;
                        case "Imsi": invalidated.Add("Imsi"); break;
                        case "Eid": invalidated.Add("Eid"); break;
                        case "OperatorIdentifier": invalidated.Add("OperatorIdentifier"); break;
                        case "OperatorName": invalidated.Add("OperatorName"); break;
                        case "EmergencyNumbers": invalidated.Add("EmergencyNumbers"); break;
                        case "PreferredNetworks": invalidated.Add("PreferredNetworks"); break;
                        case "Gid1": invalidated.Add("Gid1"); break;
                        case "Gid2": invalidated.Add("Gid2"); break;
                        case "SimType": invalidated.Add("SimType"); break;
                        case "EsimStatus": invalidated.Add("EsimStatus"); break;
                        case "Removability": invalidated.Add("Removability"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SimProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SimProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Active":
                        reader.ReadSignature("b");
                        props.Active = reader.ReadBool();
                        changedList?.Add("Active");
                        break;
                    case "SimIdentifier":
                        reader.ReadSignature("s");
                        props.SimIdentifier = reader.ReadString();
                        changedList?.Add("SimIdentifier");
                        break;
                    case "Imsi":
                        reader.ReadSignature("s");
                        props.Imsi = reader.ReadString();
                        changedList?.Add("Imsi");
                        break;
                    case "Eid":
                        reader.ReadSignature("s");
                        props.Eid = reader.ReadString();
                        changedList?.Add("Eid");
                        break;
                    case "OperatorIdentifier":
                        reader.ReadSignature("s");
                        props.OperatorIdentifier = reader.ReadString();
                        changedList?.Add("OperatorIdentifier");
                        break;
                    case "OperatorName":
                        reader.ReadSignature("s");
                        props.OperatorName = reader.ReadString();
                        changedList?.Add("OperatorName");
                        break;
                    case "EmergencyNumbers":
                        reader.ReadSignature("as");
                        props.EmergencyNumbers = reader.ReadArrayOfString();
                        changedList?.Add("EmergencyNumbers");
                        break;
                    case "PreferredNetworks":
                        reader.ReadSignature("a(su)");
                        props.PreferredNetworks = ReadType_arsuz(ref reader);
                        changedList?.Add("PreferredNetworks");
                        break;
                    case "Gid1":
                        reader.ReadSignature("ay");
                        props.Gid1 = reader.ReadArrayOfByte();
                        changedList?.Add("Gid1");
                        break;
                    case "Gid2":
                        reader.ReadSignature("ay");
                        props.Gid2 = reader.ReadArrayOfByte();
                        changedList?.Add("Gid2");
                        break;
                    case "SimType":
                        reader.ReadSignature("u");
                        props.SimType = reader.ReadUInt32();
                        changedList?.Add("SimType");
                        break;
                    case "EsimStatus":
                        reader.ReadSignature("u");
                        props.EsimStatus = reader.ReadUInt32();
                        changedList?.Add("EsimStatus");
                        break;
                    case "Removability":
                        reader.ReadSignature("u");
                        props.Removability = reader.ReadUInt32();
                        changedList?.Add("Removability");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SmsProperties
    {
        public uint State { get; set; } = default!;
        public uint PduType { get; set; } = default!;
        public string Number { get; set; } = default!;
        public string Text { get; set; } = default!;
        public byte[] Data { get; set; } = default!;
        public string SMSC { get; set; } = default!;
        public (uint, VariantValue) Validity { get; set; } = default!;
        public int Class { get; set; } = default!;
        public uint TeleserviceId { get; set; } = default!;
        public uint ServiceCategory { get; set; } = default!;
        public bool DeliveryReportRequest { get; set; } = default!;
        public uint MessageReference { get; set; } = default!;
        public string Timestamp { get; set; } = default!;
        public string DischargeTimestamp { get; set; } = default!;
        public uint DeliveryState { get; set; } = default!;
        public uint Storage { get; set; } = default!;
    }
    partial class Sms : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Sms";
        public Sms(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task SendAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Send");
                return writer.CreateMessage();
            }
        }
        public Task StoreAsync(uint storage)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "Store");
                writer.WriteUInt32(storage);
                return writer.CreateMessage();
            }
        }
        public Task SetStateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("State");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPduTypeAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("PduType");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetNumberAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Number");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetTextAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Text");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDataAsync(byte[] value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Data");
                writer.WriteSignature("ay");
                writer.WriteArray(value);
                return writer.CreateMessage();
            }
        }
        public Task SetSMSCAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("SMSC");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetValidityAsync((uint, VariantValue) value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Validity");
                writer.WriteSignature("(uv)");
                WriteType_ruvz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetClassAsync(int value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Class");
                writer.WriteSignature("i");
                writer.WriteInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetTeleserviceIdAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("TeleserviceId");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetServiceCategoryAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("ServiceCategory");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDeliveryReportRequestAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("DeliveryReportRequest");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetMessageReferenceAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("MessageReference");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetTimestampAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Timestamp");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDischargeTimestampAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("DischargeTimestamp");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetDeliveryStateAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("DeliveryState");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetStorageAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Storage");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task<uint> GetStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetPduTypeAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PduType"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<string> GetNumberAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Number"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetTextAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Text"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<byte[]> GetDataAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Data"), (Message m, object? s) => ReadMessage_v_ay(m, (ModemManager1Object)s!), this);
        public Task<string> GetSMSCAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SMSC"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<(uint, VariantValue)> GetValidityAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Validity"), (Message m, object? s) => ReadMessage_v_ruvz(m, (ModemManager1Object)s!), this);
        public Task<int> GetClassAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Class"), (Message m, object? s) => ReadMessage_v_i(m, (ModemManager1Object)s!), this);
        public Task<uint> GetTeleserviceIdAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "TeleserviceId"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetServiceCategoryAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ServiceCategory"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<bool> GetDeliveryReportRequestAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DeliveryReportRequest"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<uint> GetMessageReferenceAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "MessageReference"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<string> GetTimestampAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Timestamp"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<string> GetDischargeTimestampAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DischargeTimestamp"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<uint> GetDeliveryStateAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DeliveryState"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetStorageAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Storage"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<SmsProperties> GetPropertiesAsync()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static SmsProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SmsProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SmsProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SmsProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "State": invalidated.Add("State"); break;
                        case "PduType": invalidated.Add("PduType"); break;
                        case "Number": invalidated.Add("Number"); break;
                        case "Text": invalidated.Add("Text"); break;
                        case "Data": invalidated.Add("Data"); break;
                        case "SMSC": invalidated.Add("SMSC"); break;
                        case "Validity": invalidated.Add("Validity"); break;
                        case "Class": invalidated.Add("Class"); break;
                        case "TeleserviceId": invalidated.Add("TeleserviceId"); break;
                        case "ServiceCategory": invalidated.Add("ServiceCategory"); break;
                        case "DeliveryReportRequest": invalidated.Add("DeliveryReportRequest"); break;
                        case "MessageReference": invalidated.Add("MessageReference"); break;
                        case "Timestamp": invalidated.Add("Timestamp"); break;
                        case "DischargeTimestamp": invalidated.Add("DischargeTimestamp"); break;
                        case "DeliveryState": invalidated.Add("DeliveryState"); break;
                        case "Storage": invalidated.Add("Storage"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SmsProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SmsProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "State":
                        reader.ReadSignature("u");
                        props.State = reader.ReadUInt32();
                        changedList?.Add("State");
                        break;
                    case "PduType":
                        reader.ReadSignature("u");
                        props.PduType = reader.ReadUInt32();
                        changedList?.Add("PduType");
                        break;
                    case "Number":
                        reader.ReadSignature("s");
                        props.Number = reader.ReadString();
                        changedList?.Add("Number");
                        break;
                    case "Text":
                        reader.ReadSignature("s");
                        props.Text = reader.ReadString();
                        changedList?.Add("Text");
                        break;
                    case "Data":
                        reader.ReadSignature("ay");
                        props.Data = reader.ReadArrayOfByte();
                        changedList?.Add("Data");
                        break;
                    case "SMSC":
                        reader.ReadSignature("s");
                        props.SMSC = reader.ReadString();
                        changedList?.Add("SMSC");
                        break;
                    case "Validity":
                        reader.ReadSignature("(uv)");
                        props.Validity = ReadType_ruvz(ref reader);
                        changedList?.Add("Validity");
                        break;
                    case "Class":
                        reader.ReadSignature("i");
                        props.Class = reader.ReadInt32();
                        changedList?.Add("Class");
                        break;
                    case "TeleserviceId":
                        reader.ReadSignature("u");
                        props.TeleserviceId = reader.ReadUInt32();
                        changedList?.Add("TeleserviceId");
                        break;
                    case "ServiceCategory":
                        reader.ReadSignature("u");
                        props.ServiceCategory = reader.ReadUInt32();
                        changedList?.Add("ServiceCategory");
                        break;
                    case "DeliveryReportRequest":
                        reader.ReadSignature("b");
                        props.DeliveryReportRequest = reader.ReadBool();
                        changedList?.Add("DeliveryReportRequest");
                        break;
                    case "MessageReference":
                        reader.ReadSignature("u");
                        props.MessageReference = reader.ReadUInt32();
                        changedList?.Add("MessageReference");
                        break;
                    case "Timestamp":
                        reader.ReadSignature("s");
                        props.Timestamp = reader.ReadString();
                        changedList?.Add("Timestamp");
                        break;
                    case "DischargeTimestamp":
                        reader.ReadSignature("s");
                        props.DischargeTimestamp = reader.ReadString();
                        changedList?.Add("DischargeTimestamp");
                        break;
                    case "DeliveryState":
                        reader.ReadSignature("u");
                        props.DeliveryState = reader.ReadUInt32();
                        changedList?.Add("DeliveryState");
                        break;
                    case "Storage":
                        reader.ReadSignature("u");
                        props.Storage = reader.ReadUInt32();
                        changedList?.Add("Storage");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record BearerProperties
    {
        public string Interface { get; set; } = default!;
        public bool Connected { get; set; } = default!;
        public (string, string) ConnectionError { get; set; } = default!;
        public bool Suspended { get; set; } = default!;
        public bool Multiplexed { get; set; } = default!;
        public Dictionary<string, VariantValue> Ip4Config { get; set; } = default!;
        public Dictionary<string, VariantValue> Ip6Config { get; set; } = default!;
        public Dictionary<string, VariantValue> Stats { get; set; } = default!;
        public bool ReloadStatsSupported { get; set; } = default!;
        public uint IpTimeout { get; set; } = default!;
        public uint BearerType { get; set; } = default!;
        public int ProfileId { get; set; } = default!;
        public Dictionary<string, VariantValue> Properties { get; set; } = default!;
    }
    partial class Bearer : ModemManager1Object
    {
        private const string __Interface = "org.freedesktop.ModemManager1.Bearer";
        public Bearer(ModemManager1Service service, ObjectPath path) : base(service, path)
        { }
        public Task ConnectAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Connect");
                return writer.CreateMessage();
            }
        }
        public Task DisconnectAsync()
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Disconnect");
                return writer.CreateMessage();
            }
        }
        public Task SetInterfaceAsync(string value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Interface");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetConnectedAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Connected");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetConnectionErrorAsync((string, string) value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("ConnectionError");
                writer.WriteSignature("(ss)");
                WriteType_rssz(ref writer, value);
                return writer.CreateMessage();
            }
        }
        public Task SetSuspendedAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Suspended");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetMultiplexedAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Multiplexed");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetIp4ConfigAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Ip4Config");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetIp6ConfigAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Ip6Config");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetStatsAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Stats");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task SetReloadStatsSupportedAsync(bool value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("ReloadStatsSupported");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetIpTimeoutAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("IpTimeout");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetBearerTypeAsync(uint value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("BearerType");
                writer.WriteSignature("u");
                writer.WriteUInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetProfileIdAsync(int value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("ProfileId");
                writer.WriteSignature("i");
                writer.WriteInt32(value);
                return writer.CreateMessage();
            }
        }
        public Task SetPropertiesAsync(Dictionary<string, VariantValue> value)
        {
            return this.Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = this.Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("Properties");
                writer.WriteSignature("a{sv}");
                writer.WriteDictionary(value);
                return writer.CreateMessage();
            }
        }
        public Task<string> GetInterfaceAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Interface"), (Message m, object? s) => ReadMessage_v_s(m, (ModemManager1Object)s!), this);
        public Task<bool> GetConnectedAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Connected"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<(string, string)> GetConnectionErrorAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ConnectionError"), (Message m, object? s) => ReadMessage_v_rssz(m, (ModemManager1Object)s!), this);
        public Task<bool> GetSuspendedAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Suspended"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<bool> GetMultiplexedAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Multiplexed"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetIp4ConfigAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Ip4Config"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetIp6ConfigAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Ip6Config"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetStatsAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Stats"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<bool> GetReloadStatsSupportedAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ReloadStatsSupported"), (Message m, object? s) => ReadMessage_v_b(m, (ModemManager1Object)s!), this);
        public Task<uint> GetIpTimeoutAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IpTimeout"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<uint> GetBearerTypeAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "BearerType"), (Message m, object? s) => ReadMessage_v_u(m, (ModemManager1Object)s!), this);
        public Task<int> GetProfileIdAsync()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ProfileId"), (Message m, object? s) => ReadMessage_v_i(m, (ModemManager1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetPropertiesAsyncMethod1()
            => this.Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Properties"), (Message m, object? s) => ReadMessage_v_aesv(m, (ModemManager1Object)s!), this);
        public Task<BearerProperties> GetPropertiesAsyncMethod2()
        {
            return this.Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), this);
            static BearerProperties ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<BearerProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return base.WatchPropertiesChangedAsync(__Interface, (Message m, object? s) => ReadMessage(m, (ModemManager1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<BearerProperties> ReadMessage(Message message, ModemManager1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<BearerProperties>(ReadProperties(ref reader, changed), changed.ToArray(), ReadInvalidated(ref reader));
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Interface": invalidated.Add("Interface"); break;
                        case "Connected": invalidated.Add("Connected"); break;
                        case "ConnectionError": invalidated.Add("ConnectionError"); break;
                        case "Suspended": invalidated.Add("Suspended"); break;
                        case "Multiplexed": invalidated.Add("Multiplexed"); break;
                        case "Ip4Config": invalidated.Add("Ip4Config"); break;
                        case "Ip6Config": invalidated.Add("Ip6Config"); break;
                        case "Stats": invalidated.Add("Stats"); break;
                        case "ReloadStatsSupported": invalidated.Add("ReloadStatsSupported"); break;
                        case "IpTimeout": invalidated.Add("IpTimeout"); break;
                        case "BearerType": invalidated.Add("BearerType"); break;
                        case "ProfileId": invalidated.Add("ProfileId"); break;
                        case "Properties": invalidated.Add("Properties"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static BearerProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new BearerProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Interface":
                        reader.ReadSignature("s");
                        props.Interface = reader.ReadString();
                        changedList?.Add("Interface");
                        break;
                    case "Connected":
                        reader.ReadSignature("b");
                        props.Connected = reader.ReadBool();
                        changedList?.Add("Connected");
                        break;
                    case "ConnectionError":
                        reader.ReadSignature("(ss)");
                        props.ConnectionError = ReadType_rssz(ref reader);
                        changedList?.Add("ConnectionError");
                        break;
                    case "Suspended":
                        reader.ReadSignature("b");
                        props.Suspended = reader.ReadBool();
                        changedList?.Add("Suspended");
                        break;
                    case "Multiplexed":
                        reader.ReadSignature("b");
                        props.Multiplexed = reader.ReadBool();
                        changedList?.Add("Multiplexed");
                        break;
                    case "Ip4Config":
                        reader.ReadSignature("a{sv}");
                        props.Ip4Config = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Ip4Config");
                        break;
                    case "Ip6Config":
                        reader.ReadSignature("a{sv}");
                        props.Ip6Config = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Ip6Config");
                        break;
                    case "Stats":
                        reader.ReadSignature("a{sv}");
                        props.Stats = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Stats");
                        break;
                    case "ReloadStatsSupported":
                        reader.ReadSignature("b");
                        props.ReloadStatsSupported = reader.ReadBool();
                        changedList?.Add("ReloadStatsSupported");
                        break;
                    case "IpTimeout":
                        reader.ReadSignature("u");
                        props.IpTimeout = reader.ReadUInt32();
                        changedList?.Add("IpTimeout");
                        break;
                    case "BearerType":
                        reader.ReadSignature("u");
                        props.BearerType = reader.ReadUInt32();
                        changedList?.Add("BearerType");
                        break;
                    case "ProfileId":
                        reader.ReadSignature("i");
                        props.ProfileId = reader.ReadInt32();
                        changedList?.Add("ProfileId");
                        break;
                    case "Properties":
                        reader.ReadSignature("a{sv}");
                        props.Properties = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("Properties");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    partial class ModemManager1Service
    {
        public Tmds.DBus.Protocol.Connection Connection { get; }
        public string Destination { get; }
        public ModemManager1Service(Tmds.DBus.Protocol.Connection connection, string destination)
            => (Connection, Destination) = (connection, destination);
        public ObjectManager CreateObjectManager(string path) => new ObjectManager(this, path);
        public ModemManager1 CreateModemManager1(string path) => new ModemManager1(this, path);
        public Location CreateLocation(string path) => new Location(this, path);
        public Signal CreateSignal(string path) => new Signal(this, path);
        public Ussd CreateUssd(string path) => new Ussd(this, path);
        public Messaging CreateMessaging(string path) => new Messaging(this, path);
        public Modem CreateModem(string path) => new Modem(this, path);
        public Time CreateTime(string path) => new Time(this, path);
        public Firmware CreateFirmware(string path) => new Firmware(this, path);
        public ProfileManager CreateProfileManager(string path) => new ProfileManager(this, path);
        public Sar CreateSar(string path) => new Sar(this, path);
        public Simple CreateSimple(string path) => new Simple(this, path);
        public Modem3gpp CreateModem3gpp(string path) => new Modem3gpp(this, path);
        public Voice CreateVoice(string path) => new Voice(this, path);
        public Sim CreateSim(string path) => new Sim(this, path);
        public Sms CreateSms(string path) => new Sms(this, path);
        public Bearer CreateBearer(string path) => new Bearer(this, path);
    }
    class ModemManager1Object
    {
        public ModemManager1Service Service { get; }
        public ObjectPath Path { get; }
        protected Tmds.DBus.Protocol.Connection Connection => Service.Connection;
        protected ModemManager1Object(ModemManager1Service service, ObjectPath path)
            => (Service, Path) = (service, path);
        protected MessageBuffer CreateGetPropertyMessage(string @interface, string property)
        {
            var writer = this.Connection.GetMessageWriter();
            writer.WriteMethodCallHeader(
                destination: Service.Destination,
                path: Path,
                @interface: "org.freedesktop.DBus.Properties",
                signature: "ss",
                member: "Get");
            writer.WriteString(@interface);
            writer.WriteString(property);
            return writer.CreateMessage();
        }
        protected MessageBuffer CreateGetAllPropertiesMessage(string @interface)
        {
            var writer = this.Connection.GetMessageWriter();
            writer.WriteMethodCallHeader(
                destination: Service.Destination,
                path: Path,
                @interface: "org.freedesktop.DBus.Properties",
                signature: "s",
                member: "GetAll");
            writer.WriteString(@interface);
            return writer.CreateMessage();
        }
        protected ValueTask<IDisposable> WatchPropertiesChangedAsync<TProperties>(string @interface, MessageValueReader<PropertyChanges<TProperties>> reader, Action<Exception?, PropertyChanges<TProperties>> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = Service.Destination,
                Path = Path,
                Interface = "org.freedesktop.DBus.Properties",
                Member = "PropertiesChanged",
                Arg0 = @interface
            };
            return this.Connection.AddMatchAsync(rule, reader,
                                                    (Exception? ex, PropertyChanges<TProperties> changes, object? rs, object? hs) => ((Action<Exception?, PropertyChanges<TProperties>>)hs!).Invoke(ex, changes),
                                                    this, handler, emitOnCapturedContext, flags);
        }
        public ValueTask<IDisposable> WatchSignalAsync<TArg>(string sender, string @interface, ObjectPath path, string signal, MessageValueReader<TArg> reader, Action<Exception?, TArg> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = sender,
                Path = path,
                Member = signal,
                Interface = @interface
            };
            return this.Connection.AddMatchAsync(rule, reader,
                                                    (Exception? ex, TArg arg, object? rs, object? hs) => ((Action<Exception?, TArg>)hs!).Invoke(ex, arg),
                                                    this, handler, emitOnCapturedContext, flags);
        }
        public ValueTask<IDisposable> WatchSignalAsync(string sender, string @interface, ObjectPath path, string signal, Action<Exception?> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = sender,
                Path = path,
                Member = signal,
                Interface = @interface
            };
            return this.Connection.AddMatchAsync<object>(rule, (Message message, object? state) => null!,
                                                            (Exception? ex, object v, object? rs, object? hs) => ((Action<Exception?>)hs!).Invoke(ex), this, handler, emitOnCapturedContext, flags);
        }
        protected static Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>> ReadMessage_aeoaesaesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_aeoaesaesv(ref reader);
        }
        protected static (ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>) ReadMessage_oaesaesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadObjectPath();
            var arg1 = ReadType_aesaesv(ref reader);
            return (arg0, arg1);
        }
        protected static (ObjectPath, string[]) ReadMessage_oas(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadObjectPath();
            var arg1 = reader.ReadArrayOfString();
            return (arg0, arg1);
        }
        protected static string ReadMessage_v_s(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("s");
            return reader.ReadString();
        }
        protected static Dictionary<uint, VariantValue> ReadMessage_aeuv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_aeuv(ref reader);
        }
        protected static uint ReadMessage_v_u(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("u");
            return reader.ReadUInt32();
        }
        protected static bool ReadMessage_v_b(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("b");
            return reader.ReadBool();
        }
        protected static Dictionary<uint, VariantValue> ReadMessage_v_aeuv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a{uv}");
            return ReadType_aeuv(ref reader);
        }
        protected static string[] ReadMessage_v_as(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("as");
            return reader.ReadArrayOfString();
        }
        protected static Dictionary<string, VariantValue> ReadMessage_v_aesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a{sv}");
            return reader.ReadDictionaryOfStringToVariantValue();
        }
        protected static string ReadMessage_s(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadString();
        }
        protected static ObjectPath[] ReadMessage_ao(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadArrayOfObjectPath();
        }
        protected static ObjectPath ReadMessage_o(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadObjectPath();
        }
        protected static (ObjectPath, bool) ReadMessage_ob(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadObjectPath();
            var arg1 = reader.ReadBool();
            return (arg0, arg1);
        }
        protected static ObjectPath[] ReadMessage_v_ao(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("ao");
            return reader.ReadArrayOfObjectPath();
        }
        protected static uint[] ReadMessage_v_au(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("au");
            return reader.ReadArrayOfUInt32();
        }
        protected static Dictionary<string, VariantValue>[] ReadMessage_aaesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_aaesv(ref reader);
        }
        protected static (int, int, uint) ReadMessage_iiu(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadInt32();
            var arg1 = reader.ReadInt32();
            var arg2 = reader.ReadUInt32();
            return (arg0, arg1, arg2);
        }
        protected static ObjectPath ReadMessage_v_o(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("o");
            return reader.ReadObjectPath();
        }
        protected static (string, uint)[] ReadMessage_v_arsuz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a(su)");
            return ReadType_arsuz(ref reader);
        }
        protected static Dictionary<uint, uint> ReadMessage_v_aeuu(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a{uu}");
            return ReadType_aeuu(ref reader);
        }
        protected static int ReadMessage_v_i(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("i");
            return reader.ReadInt32();
        }
        protected static (uint, bool) ReadMessage_v_rubz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(ub)");
            return ReadType_rubz(ref reader);
        }
        protected static (uint, uint)[] ReadMessage_v_aruuz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a(uu)");
            return ReadType_aruuz(ref reader);
        }
        protected static (uint, uint) ReadMessage_v_ruuz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(uu)");
            return ReadType_ruuz(ref reader);
        }
        protected static (string, Dictionary<string, VariantValue>[]) ReadMessage_saaesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadString();
            var arg1 = ReadType_aaesv(ref reader);
            return (arg0, arg1);
        }
        protected static (uint, Dictionary<string, VariantValue>) ReadMessage_v_ruaesvz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(ua{sv})");
            return ReadType_ruaesvz(ref reader);
        }
        protected static Dictionary<string, VariantValue> ReadMessage_aesv(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadDictionaryOfStringToVariantValue();
        }
        protected static (uint, bool, byte[])[] ReadMessage_v_arubayz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a(ubay)");
            return ReadType_arubayz(ref reader);
        }
        protected static bool ReadMessage_b(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadBool();
        }
        protected static byte[] ReadMessage_v_ay(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("ay");
            return reader.ReadArrayOfByte();
        }
        protected static (uint, VariantValue) ReadMessage_v_ruvz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(uv)");
            return ReadType_ruvz(ref reader);
        }
        protected static (string, string) ReadMessage_v_rssz(Message message, ModemManager1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(ss)");
            return ReadType_rssz(ref reader);
        }
        protected static Dictionary<uint, VariantValue> ReadType_aeuv(ref Reader reader)
        {
            Dictionary<uint, VariantValue> dictionary = new();
            ArrayEnd dictEnd = reader.ReadDictionaryStart();
            while (reader.HasNext(dictEnd))
            {
                var key = reader.ReadUInt32();
                var value = reader.ReadVariantValue();
                dictionary[key] = value;
            }
            return dictionary;
        }
        protected static (string, uint)[] ReadType_arsuz(ref Reader reader)
        {
            List<(string, uint)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rsuz(ref reader));
            }
            return list.ToArray();
        }
        protected static (string, uint) ReadType_rsuz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadUInt32());
        }
        protected static Dictionary<uint, uint> ReadType_aeuu(ref Reader reader)
        {
            Dictionary<uint, uint> dictionary = new();
            ArrayEnd dictEnd = reader.ReadDictionaryStart();
            while (reader.HasNext(dictEnd))
            {
                var key = reader.ReadUInt32();
                var value = reader.ReadUInt32();
                dictionary[key] = value;
            }
            return dictionary;
        }
        protected static (uint, bool) ReadType_rubz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadBool());
        }
        protected static (uint, uint)[] ReadType_aruuz(ref Reader reader)
        {
            List<(uint, uint)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_ruuz(ref reader));
            }
            return list.ToArray();
        }
        protected static (uint, uint) ReadType_ruuz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadUInt32());
        }
        protected static (uint, Dictionary<string, VariantValue>) ReadType_ruaesvz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadDictionaryOfStringToVariantValue());
        }
        protected static (uint, bool, byte[])[] ReadType_arubayz(ref Reader reader)
        {
            List<(uint, bool, byte[])> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rubayz(ref reader));
            }
            return list.ToArray();
        }
        protected static (uint, bool, byte[]) ReadType_rubayz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadBool(), reader.ReadArrayOfByte());
        }
        protected static (uint, VariantValue) ReadType_ruvz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadVariantValue());
        }
        protected static (string, string) ReadType_rssz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadString());
        }
        protected static Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>> ReadType_aeoaesaesv(ref Reader reader)
        {
            Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>> dictionary = new();
            ArrayEnd dictEnd = reader.ReadDictionaryStart();
            while (reader.HasNext(dictEnd))
            {
                var key = reader.ReadObjectPath();
                var value = ReadType_aesaesv(ref reader);
                dictionary[key] = value;
            }
            return dictionary;
        }
        protected static Dictionary<string, Dictionary<string, VariantValue>> ReadType_aesaesv(ref Reader reader)
        {
            Dictionary<string, Dictionary<string, VariantValue>> dictionary = new();
            ArrayEnd dictEnd = reader.ReadDictionaryStart();
            while (reader.HasNext(dictEnd))
            {
                var key = reader.ReadString();
                var value = reader.ReadDictionaryOfStringToVariantValue();
                dictionary[key] = value;
            }
            return dictionary;
        }
        protected static Dictionary<string, VariantValue>[] ReadType_aaesv(ref Reader reader)
        {
            List<Dictionary<string, VariantValue>> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Array);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(reader.ReadDictionaryOfStringToVariantValue());
            }
            return list.ToArray();
        }
        protected static void WriteType_aeuv(ref MessageWriter writer, Dictionary<uint, VariantValue> value)
        {
            ArrayStart arrayStart = writer.WriteDictionaryStart();
            foreach (var item in value)
            {
                writer.WriteDictionaryEntryStart();
                writer.WriteUInt32(item.Key);
                writer.WriteVariant(item.Value);
            }
            writer.WriteDictionaryEnd(arrayStart);
        }
        protected static void WriteType_ruuz(ref MessageWriter writer, (uint, uint) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteUInt32(value.Item2);
        }
        protected static void WriteType_arsuz(ref MessageWriter writer, (string, uint)[] value)
        {
            ArrayStart arrayStart = writer.WriteArrayStart(DBusType.Struct);
            foreach (var item in value)
            {
                WriteType_rsuz(ref writer, item);
            }
            writer.WriteArrayEnd(arrayStart);
        }
        protected static void WriteType_rsuz(ref MessageWriter writer, (string, uint) value)
        {
            writer.WriteStructureStart();
            writer.WriteString(value.Item1);
            writer.WriteUInt32(value.Item2);
        }
        protected static void WriteType_aeuu(ref MessageWriter writer, Dictionary<uint, uint> value)
        {
            ArrayStart arrayStart = writer.WriteDictionaryStart();
            foreach (var item in value)
            {
                writer.WriteDictionaryEntryStart();
                writer.WriteUInt32(item.Key);
                writer.WriteUInt32(item.Value);
            }
            writer.WriteDictionaryEnd(arrayStart);
        }
        protected static void WriteType_rubz(ref MessageWriter writer, (uint, bool) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteBool(value.Item2);
        }
        protected static void WriteType_aruuz(ref MessageWriter writer, (uint, uint)[] value)
        {
            ArrayStart arrayStart = writer.WriteArrayStart(DBusType.Struct);
            foreach (var item in value)
            {
                WriteType_ruuz(ref writer, item);
            }
            writer.WriteArrayEnd(arrayStart);
        }
        protected static void WriteType_ruaesvz(ref MessageWriter writer, (uint, Dictionary<string, VariantValue>) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteDictionary(value.Item2);
        }
        protected static void WriteType_rusz(ref MessageWriter writer, (uint, string) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteString(value.Item2);
        }
        protected static void WriteType_arubayz(ref MessageWriter writer, (uint, bool, byte[])[] value)
        {
            ArrayStart arrayStart = writer.WriteArrayStart(DBusType.Struct);
            foreach (var item in value)
            {
                WriteType_rubayz(ref writer, item);
            }
            writer.WriteArrayEnd(arrayStart);
        }
        protected static void WriteType_rubayz(ref MessageWriter writer, (uint, bool, byte[]) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteBool(value.Item2);
            writer.WriteArray(value.Item3);
        }
        protected static void WriteType_ruvz(ref MessageWriter writer, (uint, VariantValue) value)
        {
            writer.WriteStructureStart();
            writer.WriteUInt32(value.Item1);
            writer.WriteVariant(value.Item2);
        }
        protected static void WriteType_rssz(ref MessageWriter writer, (string, string) value)
        {
            writer.WriteStructureStart();
            writer.WriteString(value.Item1);
            writer.WriteString(value.Item2);
        }
    }
    class PropertyChanges<TProperties>
    {
        public PropertyChanges(TProperties properties, string[] invalidated, string[] changed)
            => (Properties, Invalidated, Changed) = (properties, invalidated, changed);
        public TProperties Properties { get; }
        public string[] Invalidated { get; }
        public string[] Changed { get; }
        public bool HasChanged(string property) => Array.IndexOf(Changed, property) != -1;
        public bool IsInvalidated(string property) => Array.IndexOf(Invalidated, property) != -1;
    }
}
