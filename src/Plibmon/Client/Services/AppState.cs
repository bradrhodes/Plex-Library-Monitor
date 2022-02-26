using Microsoft.AspNetCore.Components;

namespace Plibmon.Client.Services;

/// <summary>
/// This is a pattern for state in Blazor shown on Carl Franklin's Blazor Train
/// https://youtu.be/BB4lK2kfKf0?t=546 
/// </summary>
public class AppState
{
   public event Action<ComponentBase, string> StateChanged;
   private void NotifyStateChanged(ComponentBase source, string property) 
      => StateChanged?.Invoke(source, property);

   public bool CanConnectToPlex { get; private set; }

   public void UpdateCanConnectToPlex(ComponentBase source, bool value)
   {
      CanConnectToPlex = value;
      NotifyStateChanged(source, "CanConnectToPlex");
   }
}