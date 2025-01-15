using Microsoft.JSInterop;
using System.Text.Json;

namespace Community.Blazor.MapLibre;

/// <summary>
/// Represents a callback action to handle JavaScript events in C#.
/// </summary>
public class CallbackHandler
{
    private readonly IJSObjectReference _jsRuntimeReference;
    private readonly string _eventType;
    private readonly Delegate _callbackDelegate;
    private readonly Type _argumentType;

    /// <summary>
    /// Constructor for initializing a callback handler with arguments.
    /// </summary>
    /// <param name="jsRuntimeReference">Reference to the JavaScript runtime object.</param>
    /// <param name="eventType">Type of the event (e.g., click, drag).</param>
    /// <param name="callbackDelegate">The C# delegate to invoke when the event is triggered.</param>
    /// <param name="argumentType">The type of the argument expected for the callback delegate.</param>
    public CallbackHandler(IJSObjectReference jsRuntimeReference, string eventType, Delegate callbackDelegate, Type argumentType)
    {
        _jsRuntimeReference = jsRuntimeReference;
        _eventType = eventType;
        _callbackDelegate = callbackDelegate;
        _argumentType = argumentType;
    }

    /// <summary>
    /// Constructor for initializing a callback handler without arguments.
    /// </summary>
    /// <param name="jsRuntimeReference">Reference to the JavaScript runtime object.</param>
    /// <param name="eventType">Type of the event (e.g., click, drag).</param>
    /// <param name="callbackDelegate">The C# delegate to invoke when the event is triggered.</param>
    public CallbackHandler(IJSObjectReference jsRuntimeReference, string eventType, Delegate callbackDelegate)
    {
        _jsRuntimeReference = jsRuntimeReference;
        _eventType = eventType;
        _callbackDelegate = callbackDelegate;
    }

    /// <summary>
    /// Removes the event listener in JavaScript (placeholder implementation).
    /// </summary>
    public async Task RemoveAsync()
    {
        await Task.CompletedTask; // Placeholder to maintain async signature.
    }

    /// <summary>
    /// Invokes the callback with arguments from JavaScript.
    /// </summary>
    /// <param name="args">Serialized JSON arguments from JavaScript.</param>
    [JSInvokable]
    public void Invoke(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            _callbackDelegate.DynamicInvoke(); // Invoke delegate without arguments.
            return;
        }

        // Deserialize arguments into the expected type.
        var deserializedArgs = JsonSerializer.Deserialize(args, _argumentType);

        // Invoke delegate with deserialized arguments.
        _callbackDelegate.DynamicInvoke(deserializedArgs);
    }
}