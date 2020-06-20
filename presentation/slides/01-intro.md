<h2 style="position: absolute; top: 100px; color: #000; text-transform: none;">Polly: Eine "Resilience" Bibliothek</h2>
<h3 style="position: absolute; top: 300px; color: #000; text-transform: none;">(Kategorie: nützliche NuGet Pakete)</h3>

<img src="./images/Polly-Logo.png" class="borderless" style="position: relative; top: 10px; right: -400px; height: 400px">

<div style="position: absolute; top: 520px; right: -150px; color: #ccc; text-transform: none; text-align: right" class="my-shadow">
</br><i class="fa fa-calendar" aria-hidden="true"></i>&nbsp;2020-06-22
</div>

<div style="position: absolute; top: 520px; left: -145px; color: #ccc; text-transform: none; text-align: right" class="my-shadow">
    <ul style="list-style: none;">
        <li>Patrick Drechsler</li>
        <li>Redheads Ltd.</li>
    </ul>
</div>

---

![resilience-meaning](images/resilience-translation-de.png)

---

## Resilience als "1st Class Citizen"

Fehlerhaftes Verhalten von anderen Diensten 

- als "normal" betrachten und 
- flexibel darauf reagieren

---

> Polly is a .NET **resilience** and transient-fault-handling library that **allows developers to express policies such as Retry, Circuit Breaker, Timeout**, Bulkhead Isolation, and Fallback **in a fluent and thread-safe manner**.

---

## Polly Project

- stabiles, gepflegtes Repo
- große Community
- 7k Sterne auf Github
- [https://github.com/App-vNext/Polly](https://github.com/App-vNext/Polly)
- [http://www.thepollyproject.org](http://www.thepollyproject.org)

---

## Was sind Policies?

**Policies** beschreiben das Verhalten, wenn was schief geht

---

## Beispiel "Policies"

- **Timeout**
  - "Antwort dauert länger als 10sec"
- **Retry**
  - "keine Antwort -> nochmal probieren"
- **Circuit Breaker**
  - "verhindere weiter Anfragen, damit sich der Dienst erholen kann"
  - "wenn sich Dienst wieder erholt hat -> weitermachen"
- ...

---

## Polly Standardvorgehen

- Policy definieren
- eigentliche Funktion in Policy "einpacken"

---

## Beispiel Use-Case

```csharp
public class BusinessLogic
{
  private readonly IFlakyService _srv;

  public BusinessLogic(IFlakyService service) => _srv = service;

  public int CallFlakyMethod() 
  {
    return _srv.SlowMethod(); // <- SLOW!!
  }
}
```

---

## Beispiel 1: Retry

---

### Retry-Policy definieren

<pre><code data-noescape data-trim class="lang-csharp hljs">
var _policy = Policy
    <span class="highlightcode">.Handle&lt;<span style="font-weight: normal">Exception</span>&gt;()</span>
    <span class="highlightcode">.WaitAndRetry(</span><span style="font-weight: normal">3, x => TimeSpan.FromSeconds(2)</span><span class="highlightcode">);</span>
</code></pre>

- `Handle`: welche Exception?
- `WaitAndRetry`: Policy "Strategie"

---

### Policy anwenden

<pre><code data-noescape data-trim class="lang-csharp hljs">
public int CallFlakyMethod()
{
  <span class="fragment fade-in-then-out" data-fragment-index="1">return _srv.SlowMethod();</span>
  <span class="highlightcode fragment" data-fragment-index="2">return _policy.Execute(</span><span class="fragment" data-fragment-index="2" style="color: black;">() => _srv.SlowMethod()</span><span class="highlightcode fragment" data-fragment-index="2" style="color: black;">);</span>
}
</code></pre>

---

## Beispiel 2: Circuit Breaker

```csharp
var circuitBreakerPolicy = Policy
    .Handle<Exception>()
    .CircuitBreaker(1, TimeSpan.FromSeconds(1),
        (ex, t) =>
        {
            Log.Information("Circuit broken!");
        },
        () =>
        {
            Log.Information("Circuit Reset!");
        });
```

---

![screenshot-circuit-breaker](images/screenshot-circuit-breaker.png)

---

## Live-Demo

---

Policies können 

- sehr feingranular definiert werden
- miteinander kombiniert werden

---

## Fazit

Polly einsetzen, wenn man oft und/oder komplexe Policies einsetzt.
