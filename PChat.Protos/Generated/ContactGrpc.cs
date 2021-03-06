// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: contact.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Pchat {
  public static partial class Contact
  {
    static readonly string __ServiceName = "pchat.Contact";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.CallerInfo> __Marshaller_pchat_CallerInfo = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.CallerInfo.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.PeerResponse> __Marshaller_pchat_PeerResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.PeerResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.Answer> __Marshaller_pchat_Answer = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.Answer.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.CallerInfo, global::Pchat.PeerResponse> __Method_AddFriend = new grpc::Method<global::Pchat.CallerInfo, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "AddFriend",
        __Marshaller_pchat_CallerInfo,
        __Marshaller_pchat_PeerResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.Answer, global::Pchat.PeerResponse> __Method_AnswerFriendRequest = new grpc::Method<global::Pchat.Answer, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "AnswerFriendRequest",
        __Marshaller_pchat_Answer,
        __Marshaller_pchat_PeerResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Pchat.ContactReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Contact</summary>
    [grpc::BindServiceMethod(typeof(Contact), "BindService")]
    public abstract partial class ContactBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> AddFriend(global::Pchat.CallerInfo request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> AnswerFriendRequest(global::Pchat.Answer request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Contact</summary>
    public partial class ContactClient : grpc::ClientBase<ContactClient>
    {
      /// <summary>Creates a new client for Contact</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ContactClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Contact that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ContactClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ContactClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ContactClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse AddFriend(global::Pchat.CallerInfo request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AddFriend(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse AddFriend(global::Pchat.CallerInfo request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_AddFriend, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> AddFriendAsync(global::Pchat.CallerInfo request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AddFriendAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> AddFriendAsync(global::Pchat.CallerInfo request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_AddFriend, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse AnswerFriendRequest(global::Pchat.Answer request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AnswerFriendRequest(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse AnswerFriendRequest(global::Pchat.Answer request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_AnswerFriendRequest, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> AnswerFriendRequestAsync(global::Pchat.Answer request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AnswerFriendRequestAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> AnswerFriendRequestAsync(global::Pchat.Answer request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_AnswerFriendRequest, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override ContactClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ContactClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ContactBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_AddFriend, serviceImpl.AddFriend)
          .AddMethod(__Method_AnswerFriendRequest, serviceImpl.AnswerFriendRequest).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ContactBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_AddFriend, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.CallerInfo, global::Pchat.PeerResponse>(serviceImpl.AddFriend));
      serviceBinder.AddMethod(__Method_AnswerFriendRequest, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.Answer, global::Pchat.PeerResponse>(serviceImpl.AnswerFriendRequest));
    }

  }
}
#endregion
