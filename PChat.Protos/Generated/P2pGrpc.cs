// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: p2p.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Pchat {
  /// <summary>
  /// This service makes the actual connection with the target peer.
  /// </summary>
  public static partial class P2p
  {
    static readonly string __ServiceName = "pchat.P2p";

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
    static readonly grpc::Marshaller<global::Pchat.ContactCard> __Marshaller_pchat_ContactCard = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.ContactCard.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.PeerResponse> __Marshaller_pchat_PeerResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.PeerResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.TextMessage> __Marshaller_pchat_TextMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.TextMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Pchat.FriendRequest> __Marshaller_pchat_FriendRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Pchat.FriendRequest.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.ContactCard, global::Pchat.PeerResponse> __Method_ReceiveContactCard = new grpc::Method<global::Pchat.ContactCard, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReceiveContactCard",
        __Marshaller_pchat_ContactCard,
        __Marshaller_pchat_PeerResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.TextMessage, global::Pchat.PeerResponse> __Method_ReceiveMessage = new grpc::Method<global::Pchat.TextMessage, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReceiveMessage",
        __Marshaller_pchat_TextMessage,
        __Marshaller_pchat_PeerResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.TextMessage, global::Pchat.PeerResponse> __Method_ReceiveMessageUpdate = new grpc::Method<global::Pchat.TextMessage, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReceiveMessageUpdate",
        __Marshaller_pchat_TextMessage,
        __Marshaller_pchat_PeerResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.FriendRequest, global::Pchat.PeerResponse> __Method_ReceiveFriendRequest = new grpc::Method<global::Pchat.FriendRequest, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReceiveFriendRequest",
        __Marshaller_pchat_FriendRequest,
        __Marshaller_pchat_PeerResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Pchat.FriendRequest, global::Pchat.PeerResponse> __Method_ReceiveFriendRequestUpdate = new grpc::Method<global::Pchat.FriendRequest, global::Pchat.PeerResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReceiveFriendRequestUpdate",
        __Marshaller_pchat_FriendRequest,
        __Marshaller_pchat_PeerResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Pchat.P2PReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of P2p</summary>
    [grpc::BindServiceMethod(typeof(P2p), "BindService")]
    public abstract partial class P2pBase
    {
      /// <summary>
      /// Instruct peer to receive your contact card.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> ReceiveContactCard(global::Pchat.ContactCard request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Instruct peer to receive this message.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> ReceiveMessage(global::Pchat.TextMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Instruct peer to receive this message update.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> ReceiveMessageUpdate(global::Pchat.TextMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Instruct peer to receive this friend request.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> ReceiveFriendRequest(global::Pchat.FriendRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Instruct peer to receive this friend request update.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Pchat.PeerResponse> ReceiveFriendRequestUpdate(global::Pchat.FriendRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for P2p</summary>
    public partial class P2pClient : grpc::ClientBase<P2pClient>
    {
      /// <summary>Creates a new client for P2p</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public P2pClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for P2p that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public P2pClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected P2pClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected P2pClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Instruct peer to receive your contact card.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveContactCard(global::Pchat.ContactCard request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveContactCard(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive your contact card.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveContactCard(global::Pchat.ContactCard request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReceiveContactCard, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive your contact card.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveContactCardAsync(global::Pchat.ContactCard request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveContactCardAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive your contact card.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveContactCardAsync(global::Pchat.ContactCard request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReceiveContactCard, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this message.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveMessage(global::Pchat.TextMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveMessage(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this message.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveMessage(global::Pchat.TextMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReceiveMessage, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this message.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveMessageAsync(global::Pchat.TextMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveMessageAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this message.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveMessageAsync(global::Pchat.TextMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReceiveMessage, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this message update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveMessageUpdate(global::Pchat.TextMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveMessageUpdate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this message update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveMessageUpdate(global::Pchat.TextMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReceiveMessageUpdate, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this message update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveMessageUpdateAsync(global::Pchat.TextMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveMessageUpdateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this message update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveMessageUpdateAsync(global::Pchat.TextMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReceiveMessageUpdate, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this friend request.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveFriendRequest(global::Pchat.FriendRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveFriendRequest(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this friend request.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveFriendRequest(global::Pchat.FriendRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReceiveFriendRequest, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this friend request.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveFriendRequestAsync(global::Pchat.FriendRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveFriendRequestAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this friend request.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveFriendRequestAsync(global::Pchat.FriendRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReceiveFriendRequest, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this friend request update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveFriendRequestUpdate(global::Pchat.FriendRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveFriendRequestUpdate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this friend request update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Pchat.PeerResponse ReceiveFriendRequestUpdate(global::Pchat.FriendRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReceiveFriendRequestUpdate, null, options, request);
      }
      /// <summary>
      /// Instruct peer to receive this friend request update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveFriendRequestUpdateAsync(global::Pchat.FriendRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReceiveFriendRequestUpdateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Instruct peer to receive this friend request update.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Pchat.PeerResponse> ReceiveFriendRequestUpdateAsync(global::Pchat.FriendRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReceiveFriendRequestUpdate, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override P2pClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new P2pClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(P2pBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ReceiveContactCard, serviceImpl.ReceiveContactCard)
          .AddMethod(__Method_ReceiveMessage, serviceImpl.ReceiveMessage)
          .AddMethod(__Method_ReceiveMessageUpdate, serviceImpl.ReceiveMessageUpdate)
          .AddMethod(__Method_ReceiveFriendRequest, serviceImpl.ReceiveFriendRequest)
          .AddMethod(__Method_ReceiveFriendRequestUpdate, serviceImpl.ReceiveFriendRequestUpdate).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, P2pBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ReceiveContactCard, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.ContactCard, global::Pchat.PeerResponse>(serviceImpl.ReceiveContactCard));
      serviceBinder.AddMethod(__Method_ReceiveMessage, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.TextMessage, global::Pchat.PeerResponse>(serviceImpl.ReceiveMessage));
      serviceBinder.AddMethod(__Method_ReceiveMessageUpdate, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.TextMessage, global::Pchat.PeerResponse>(serviceImpl.ReceiveMessageUpdate));
      serviceBinder.AddMethod(__Method_ReceiveFriendRequest, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.FriendRequest, global::Pchat.PeerResponse>(serviceImpl.ReceiveFriendRequest));
      serviceBinder.AddMethod(__Method_ReceiveFriendRequestUpdate, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Pchat.FriendRequest, global::Pchat.PeerResponse>(serviceImpl.ReceiveFriendRequestUpdate));
    }

  }
}
#endregion
