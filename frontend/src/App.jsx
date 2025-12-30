import { useEffect, useState } from 'react'
import axios from 'axios'
import './App.css'

const API_BASE = 'http://localhost:5016/api'
const RESOURCES = ['tires', 'batteries', 'oils', 'filters', 'cars']

function App() {
  const [apiStatus, setApiStatus] = useState('checking')
  const [missingResources, setMissingResources] = useState([])
  const [data, setData] = useState({})
  const [selectedResource, setSelectedResource] = useState('tires')
  const [editingId, setEditingId] = useState(null)
  const [formData, setFormData] = useState({})
  const [loading, setLoading] = useState({})

  // Проверка доступности API
  useEffect(() => {
    checkApiStatus()
  }, [])

  const checkApiStatus = async () => {
    const missing = []
    
    for (const resource of RESOURCES) {
      try {
        await axios.get(`${API_BASE}/${resource}`, { timeout: 3000 })
      } catch (err) {
        missing.push(resource)
      }
    }

    if (missing.length === 0) {
      setApiStatus('available')
      loadAllData()
    } else if (missing.length === RESOURCES.length) {
      setApiStatus('unavailable')
      setMissingResources(RESOURCES)
    } else {
      setApiStatus('partial')
      setMissingResources(missing)
      loadAvailableData(missing)
    }
  }

  const loadAllData = async () => {
    for (const resource of RESOURCES) {
      await loadResourceData(resource)
    }
  }

  const loadAvailableData = async (missingResources) => {
    for (const resource of RESOURCES) {
      if (!missingResources.includes(resource)) {
        await loadResourceData(resource)
      }
    }
  }

  const loadResourceData = async (resource) => {
    setLoading(prev => ({ ...prev, [resource]: true }))
    try {
      const response = await axios.get(`${API_BASE}/${resource}`)
      setData(prev => ({ ...prev, [resource]: response.data }))
    } catch (err) {
      console.error(`Ошибка загрузки ${resource}:`, err.message)
      setData(prev => ({ ...prev, [resource]: [] }))
    } finally {
      setLoading(prev => ({ ...prev, [resource]: false }))
    }
  }

  const handleCreate = async () => {
    if (!formData.Name) {
      alert('Заполните название')
      return
    }

    try {
      // Преобразование данных перед отправкой
      const dataToSend = {
        Name: formData.Name || '',
        Article: formData.Article || '',
        Price: parseFloat(formData.Price) || 0,
        Quantity: parseInt(formData.Quantity) || 0,
      }

      // Специфичные поля для каждого ресурса
      if (selectedResource === 'tires') {
        dataToSend.DiameterInches = parseInt(formData.DiameterInches) || 0
        dataToSend.ProfileDescription = formData.ProfileDescription || ''
      } else if (selectedResource === 'batteries') {
        dataToSend.VoltageV = parseInt(formData.VoltageV) || 0
        dataToSend.CapacityAh = parseInt(formData.CapacityAh) || 0
      } else if (selectedResource === 'cars') {
        dataToSend.Brand = formData.Brand || ''
        dataToSend.Model = formData.Model || ''
        dataToSend.Year = parseInt(formData.Year) || new Date().getFullYear()
        dataToSend.Color = formData.Color || ''
      }

      console.log('Отправляемые данные:', dataToSend)
      await axios.post(`${API_BASE}/${selectedResource}`, dataToSend)
      setFormData({})
      await loadResourceData(selectedResource)
    } catch (err) {
      console.error('Ошибка:', err.response?.data || err.message)
      alert(`Ошибка создания: ${err.response?.data?.error || err.message}`)
    }
  }

  const handleUpdate = async (id) => {
    try {
      // Преобразование данных перед отправкой
      const dataToSend = {
        Id: id,
        Name: formData.Name || '',
        Article: formData.Article || '',
        Price: parseFloat(formData.Price) || 0,
        Quantity: parseInt(formData.Quantity) || 0,
      }

      // Специфичные поля для каждого ресурса
      if (selectedResource === 'tires') {
        dataToSend.DiameterInches = parseInt(formData.DiameterInches) || 0
        dataToSend.ProfileDescription = formData.ProfileDescription || ''
      } else if (selectedResource === 'batteries') {
        dataToSend.VoltageV = parseInt(formData.VoltageV) || 0
        dataToSend.CapacityAh = parseInt(formData.CapacityAh) || 0
      } else if (selectedResource === 'cars') {
        dataToSend.Brand = formData.Brand || ''
        dataToSend.Model = formData.Model || ''
        dataToSend.Year = parseInt(formData.Year) || new Date().getFullYear()
        dataToSend.Color = formData.Color || ''
      }

      console.log('Отправляемые данные:', dataToSend)
      await axios.put(`${API_BASE}/${selectedResource}/${id}`, dataToSend)
      setEditingId(null)
      setFormData({})
      await loadResourceData(selectedResource)
    } catch (err) {
      console.error('Ошибка:', err.response?.data || err.message)
      alert(`Ошибка обновления: ${err.response?.data?.error || err.message}`)
    }
  }

  const handleDelete = async (id) => {
    if (!window.confirm('Вы уверены?')) return

    try {
      await axios.delete(`${API_BASE}/${selectedResource}/${id}`)
      await loadResourceData(selectedResource)
    } catch (err) {
      alert(`Ошибка удаления: ${err.response?.data?.error || err.message}`)
    }
  }

  const handleEdit = (item) => {
    setEditingId(item.id)
    setFormData({ ...item })
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({ ...prev, [name]: value }))
  }

  // Интерфейс когда API недоступен
  if (apiStatus === 'unavailable') {
    return (
      <div style={{ padding: '20px', fontFamily: 'Arial' }}>
        <h1 style={{ color: 'red' }}>❌ ASP.NET Backend недоступен</h1>
        <p>Убедитесь что:</p>
        <ul>
          <li>ASP.NET приложение запущено на http://localhost:5016</li>
          <li>Все необходимые сервисы недоступны: {RESOURCES.join(', ')}</li>
        </ul>
        <button onClick={checkApiStatus}>🔄 Проверить снова</button>
      </div>
    )
  }

  // Интерфейс когда часть ресурсов недоступна
  if (apiStatus === 'partial') {
    return (
      <div style={{ padding: '20px', fontFamily: 'Arial' }}>
        <h1 style={{ color: 'orange' }}>⚠️ Частичная доступность API</h1>
        <p>Недоступные ресурсы: <strong>{missingResources.join(', ')}</strong></p>
        <p>Доступные ресурсы: <strong>{RESOURCES.filter(r => !missingResources.includes(r)).join(', ')}</strong></p>
        <button onClick={checkApiStatus}>🔄 Проверить снова</button>
        <hr />
        {apiStatus === 'partial' && renderContent()}
      </div>
    )
  }

  // Полный интерфейс когда API доступен
  return (
    <div style={{ padding: '20px', fontFamily: 'Arial' }}>
      <h1>✅ CRUD приложение</h1>
      {renderContent()}
    </div>
  )

  function renderContent() {
    const items = data[selectedResource] || []

    return (
      <div>
        <div style={{ marginBottom: '20px' }}>
          <label>Выберите ресурс: </label>
          <select value={selectedResource} onChange={(e) => {
            setSelectedResource(e.target.value)
            setEditingId(null)
            setFormData({})
          }}>
            {RESOURCES.filter(r => !missingResources.includes(r)).map(r => (
              <option key={r} value={r}>{r}</option>
            ))}
          </select>
        </div>

        <div style={{ marginBottom: '20px', padding: '10px', border: '1px solid #ccc' }}>
          <h3>{editingId ? 'Редактирование' : 'Создание новой записи'}</h3>
          <input
            type="text"
            name="Name"
            placeholder="Название"
            value={formData.Name || ''}
            onChange={handleInputChange}
            style={{ padding: '5px', marginRight: '10px' }}
          />
          {selectedResource !== 'cars' && (
            <input
              type="text"
              name="Article"
              placeholder="Артикул"
              value={formData.Article || ''}
              onChange={handleInputChange}
              style={{ padding: '5px', marginRight: '10px' }}
            />
          )}
          <input
            type="number"
            name="Price"
            placeholder="Цена"
            value={formData.Price || ''}
            onChange={handleInputChange}
            step="0.01"
            style={{ padding: '5px', marginRight: '10px' }}
          />
          {selectedResource !== 'cars' && (
            <input
              type="number"
              name="Quantity"
              placeholder="Количество"
              value={formData.Quantity || ''}
              onChange={handleInputChange}
              style={{ padding: '5px', marginRight: '10px' }}
            />
          )}
          {selectedResource === 'tires' && (
            <>
              <input
                type="number"
                name="DiameterInches"
                placeholder="Диаметр (дюймы)"
                value={formData.DiameterInches || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
              <input
                type="text"
                name="ProfileDescription"
                placeholder="Описание профиля"
                value={formData.ProfileDescription || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
            </>
          )}
          {selectedResource === 'batteries' && (
            <>
              <input
                type="number"
                name="VoltageV"
                placeholder="Напряжение (V)"
                value={formData.VoltageV || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
              <input
                type="number"
                name="CapacityAh"
                placeholder="Емкость (Ач)"
                value={formData.CapacityAh || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
            </>
          )}
          {selectedResource === 'cars' && (
            <>
              <input
                type="text"
                name="Brand"
                placeholder="Марка"
                value={formData.Brand || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
              <input
                type="text"
                name="Model"
                placeholder="Модель"
                value={formData.Model || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
              <input
                type="number"
                name="Year"
                placeholder="Год"
                value={formData.Year || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
              <input
                type="text"
                name="Color"
                placeholder="Цвет"
                value={formData.Color || ''}
                onChange={handleInputChange}
                style={{ padding: '5px', marginRight: '10px' }}
              />
            </>
          )}
          <button onClick={() => editingId ? handleUpdate(editingId) : handleCreate()}>
            {editingId ? '💾 Сохранить' : '➕ Создать'}
          </button>
          {editingId && (
            <button onClick={() => { setEditingId(null); setFormData({}); }}>
              ❌ Отмена
            </button>
          )}
        </div>

        <div>
          <h3>Список ({items.length})</h3>
          {loading[selectedResource] ? (
            <p>Загрузка...</p>
          ) : items.length === 0 ? (
            <p>Нет данных</p>
          ) : (
            <table style={{ borderCollapse: 'collapse', width: '100%' }}>
              <thead>
                <tr style={{ backgroundColor: '#f0f0f0' }}>
                  <th style={{ border: '1px solid #ddd', padding: '10px' }}>ID</th>
                  <th style={{ border: '1px solid #ddd', padding: '10px' }}>Название</th>
                  {selectedResource !== 'cars' && <th style={{ border: '1px solid #ddd', padding: '10px' }}>Артикул</th>}
                  {selectedResource === 'cars' && <th style={{ border: '1px solid #ddd', padding: '10px' }}>Марка</th>}
                  <th style={{ border: '1px solid #ddd', padding: '10px' }}>Цена</th>
                  {selectedResource === 'tires' && <th style={{ border: '1px solid #ddd', padding: '10px' }}>Диаметр</th>}
                  {selectedResource === 'batteries' && <th style={{ border: '1px solid #ddd', padding: '10px' }}>Напряжение</th>}
                  {selectedResource === 'cars' && (
                    <>
                      <th style={{ border: '1px solid #ddd', padding: '10px' }}>Модель</th>
                      <th style={{ border: '1px solid #ddd', padding: '10px' }}>Год</th>
                    </>
                  )}
                  <th style={{ border: '1px solid #ddd', padding: '10px' }}>Действия</th>
                </tr>
              </thead>
              <tbody>
                {items.map(item => (
                  <tr key={item.id}>
                    <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.id}</td>
                    <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.name}</td>
                    {selectedResource !== 'cars' && <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.article}</td>}
                    {selectedResource === 'cars' && <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.brand}</td>}
                    <td style={{ border: '1px solid #ddd', padding: '10px' }}>${item.price?.toFixed(2)}</td>
                    {selectedResource === 'tires' && <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.diameterInches}"</td>}
                    {selectedResource === 'batteries' && <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.voltageV}V</td>}
                    {selectedResource === 'cars' && (
                      <>
                        <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.model}</td>
                        <td style={{ border: '1px solid #ddd', padding: '10px' }}>{item.year}</td>
                      </>
                    )}
                    <td style={{ border: '1px solid #ddd', padding: '10px' }}>
                      <button onClick={() => handleEdit(item)}>✏️ Редакт</button>
                      <button onClick={() => handleDelete(item.id)} style={{ marginLeft: '5px' }}>🗑️ Удал</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    )
  }
}

export default App
