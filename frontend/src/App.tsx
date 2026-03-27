import { useEffect, useState } from 'react'
import './App.css'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:8080'

function App() {
  const [count, setCount] = useState<number | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const loadCounter = async () => {
    setError(null)
    const response = await fetch(`${apiBaseUrl}/counter`)
    if (!response.ok) {
      throw new Error('Failed to fetch counter')
    }

    const data = (await response.json()) as { value: number }
    setCount(data.value)
  }

  const incrementCounter = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await fetch(`${apiBaseUrl}/counter/increment`, {
        method: 'POST',
      })

      if (!response.ok) {
        throw new Error('Failed to increment counter')
      }

      await loadCounter()
    } catch {
      setError('Unable to increment counter')
    } finally {
      setIsLoading(false)
    }
  }

  const resetCounter = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await fetch(`${apiBaseUrl}/counter/reset`, {
        method: 'POST',
      })

      if (!response.ok) {
        throw new Error('Failed to reset counter')
      }

      await loadCounter()
    } catch {
      setError('Unable to reset counter')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    loadCounter().catch(() => setError('Unable to load counter'))
  }, [])

  return (
    <main className="container">
      <h1>Counter Sandbox</h1>
      <p className="counter-value">
        Current value: <strong>{count ?? '...'}</strong>
      </p>
      <button className="increment-button" onClick={incrementCounter} disabled={isLoading}>
        {isLoading ? 'Incrementing...' : 'Increment'}
      </button>
      <button className="increment-button" onClick={resetCounter} disabled={isLoading}>
        {isLoading ? 'Resetting...' : 'Reset'}
      </button>
      {error && <p className="error-message">{error}</p>}
      <p className="hint">API base URL: {apiBaseUrl}</p>
    </main>
  )
}

export default App
