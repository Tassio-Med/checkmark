import React, { useState, useEffect } from 'react';
import { Checkmark, PriorityLevel } from '../../models/Checkmark';
import { checkmarkApi } from '../../services/api';
import CheckmarkItem from '../CheckmarkItem/CheckmarkItem';
import './CheckmarkList.css';

interface CheckmarkListProps {
  onEditCheckmark?: (checkmark: Checkmark) => void;
}

const CheckmarkList: React.FC<CheckmarkListProps> = ({ onEditCheckmark }) => {
  const [checkmarks, setCheckmarks] = useState<Checkmark[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [filter, setFilter] = useState<'all' | 'completed' | 'pending'>('all');
  const [sortBy, setSortBy] = useState<'created' | 'dueDate' | 'priority' | 'title'>('created');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');

  useEffect(() => {
    loadCheckmarks();
  }, []);

  const loadCheckmarks = async (): Promise<void> => {
    try {
      setLoading(true);
      setError(null);
      
      const data = await checkmarkApi.getAll();
      setCheckmarks(data);
    } catch (err) {
      setError('Erro ao carregar os checkmarks. Verifique se o backend está rodando.');
      console.error('Erro:', err);
    } finally {
      setLoading(false);
    }
  };

  const getFilteredCheckmarks = (): Checkmark[] => {
    let filtered = checkmarks;

    switch (filter) {
      case 'completed':
        filtered = filtered.filter(item => item.isCompleted);
        break;
      case 'pending':
        filtered = filtered.filter(item => !item.isCompleted);
        break;
      default:
        break;
    }

    return filtered;
  };

  const sortCheckmarks = (items: Checkmark[]): Checkmark[] => {
  return [...items].sort((a, b) => {
    let aValue: any;
    let bValue: any;

    switch (sortBy) {
      case 'title':
        aValue = a.title.toLowerCase();
        bValue = b.title.toLowerCase();
        break;
      case 'dueDate':
        aValue = a.dueDate ? new Date(a.dueDate).getTime() : Number.MAX_SAFE_INTEGER;
        bValue = b.dueDate ? new Date(b.dueDate).getTime() : Number.MAX_SAFE_INTEGER;
        break;
      case 'priority':
        aValue = a.priority; 
        bValue = b.priority; 
        break;
      case 'created':
      default:
        aValue = new Date(a.createdAt).getTime();
        bValue = new Date(b.createdAt).getTime();
        break;
    }

    if (aValue < bValue) return sortOrder === 'asc' ? -1 : 1;
    if (aValue > bValue) return sortOrder === 'asc' ? 1 : -1;
    return 0;
    });
  };

  const toggleComplete = async (checkmark: Checkmark): Promise<void> => {
    try {
      const updatedCheckmark = {
        ...checkmark,
        isCompleted: !checkmark.isCompleted,
        updatedAt: new Date().toISOString()
      };

      await checkmarkApi.update(checkmark.id, updatedCheckmark);
      
      setCheckmarks(prevCheckmarks =>
        prevCheckmarks.map(item =>
          item.id === checkmark.id ? updatedCheckmark : item
        )
      );
    } catch (err) {
      setError('Erro ao atualizar o checkmark.');
      console.error('Erro:', err);
    }
  };

  const deleteCheckmark = async (id: number): Promise<void> => {
    try {
      await checkmarkApi.delete(id);
      
      setCheckmarks(prevCheckmarks =>
        prevCheckmarks.filter(item => item.id !== id)
      );
    } catch (err) {
      setError('Erro ao deletar o checkmark.');
      console.error('Erro:', err);
    }
  };

  const handleEditCheckmark = (checkmark: Checkmark) => {
    if (onEditCheckmark) {
      onEditCheckmark(checkmark);
    }
  };

  const completedCount = checkmarks.filter(item => item.isCompleted).length;
  const pendingCount = checkmarks.filter(item => !item.isCompleted).length;
  const overdueCount = checkmarks.filter(item => 
    item.dueDate && 
    new Date(item.dueDate) < new Date() && 
    !item.isCompleted
  ).length;

  const filteredCheckmarks = getFilteredCheckmarks();
  const sortedCheckmarks = sortCheckmarks(filteredCheckmarks);

  if (loading) {
    return (
      <div className="checkmark-list loading">
        <div className="spinner"></div>
        <p>Carregando checkmarks...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="checkmark-list error">
        <div className="error-icon">⚠️</div>
        <p>{error}</p>
        <button onClick={loadCheckmarks} className="retry-btn">
          Tentar Novamente
        </button>
      </div>
    );
  }

  return (
    <div className="checkmark-list">
      <div className="checkmarks-header">
        <div className="stats">
          <h2>Lista de Checkmarks</h2>
          <div className="stats-grid">
            <span className="stat total">
              <strong>Total:</strong> {checkmarks.length}
            </span>
            <span className="stat completed">
              <strong>Concluídos:</strong> {completedCount}
            </span>
            <span className="stat pending">
              <strong>Pendentes:</strong> {pendingCount}
            </span>
            <span className="stat overdue">
              <strong>Atrasados:</strong> {overdueCount}
            </span>
          </div>
        </div>
        
        <div className="header-actions">
          <div className="controls">
            <div className="filter-section">
              <label>Filtrar:</label>
              <div className="filter-buttons">
                <button 
                  className={`filter-btn ${filter === 'all' ? 'active' : ''}`}
                  onClick={() => setFilter('all')}
                >
                  Todos
                </button>
                <button 
                  className={`filter-btn ${filter === 'pending' ? 'active' : ''}`}
                  onClick={() => setFilter('pending')}
                >
                  Pendentes
                </button>
                <button 
                  className={`filter-btn ${filter === 'completed' ? 'active' : ''}`}
                  onClick={() => setFilter('completed')}
                >
                  Concluídos
                </button>
              </div>
            </div>

            <div className="sort-section">
              <label>Ordenar por:</label>
              <div className="sort-controls">
                <select 
                  value={sortBy}
                  onChange={(e) => setSortBy(e.target.value as any)}
                  className="sort-select"
                >
                  <option value="created">Data de Criação</option>
                  <option value="dueDate">Data de Vencimento</option>
                  <option value="priority">Prioridade</option>
                  <option value="title">Título</option>
                </select>
                
                <button 
                  onClick={() => setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc')}
                  className="sort-order-btn"
                  title={sortOrder === 'asc' ? 'Ordenar Decrescente' : 'Ordenar Crescente'}
                >
                  {sortOrder === 'asc' ? '↑' : '↓'}
                </button>
              </div>
            </div>
          </div>
          
          <button onClick={loadCheckmarks} className="refresh-btn">
            🔄 Atualizar
          </button>
        </div>
      </div>

      <div className="filters-summary">
        <span className="results-count">
          Mostrando {sortedCheckmarks.length} de {checkmarks.length} checkmarks
          {filter !== 'all' && ` (filtrado por: ${filter === 'completed' ? 'Concluídos' : 'Pendentes'})`}
        </span>
        <span className="sort-info">
          Ordenado por: {
            sortBy === 'created' ? 'Data de Criação' :
            sortBy === 'dueDate' ? 'Data de Vencimento' :
            sortBy === 'priority' ? 'Prioridade' : 'Título'
          } ({sortOrder === 'asc' ? 'Crescente' : 'Decrescente'})
        </span>
      </div>

      {sortedCheckmarks.length === 0 ? (
        <div className="no-items">
          <div className="no-items-icon">
            {filter === 'all' ? '📝' : filter === 'completed' ? '✅' : '⏳'}
          </div>
          <h3>
            {filter === 'all' 
              ? 'Nenhum checkmark encontrado'
              : filter === 'completed'
              ? 'Nenhum checkmark concluído'
              : 'Nenhum checkmark pendente'
            }
          </h3>
          <p>
            {filter === 'all' 
              ? 'Comece criando seu primeiro checkmark!'
              : filter === 'completed'
              ? 'Complete algumas tarefas para vê-las aqui.'
              : 'Todas as tarefas estão concluídas! 🎉'
            }
          </p>
        </div>
      ) : (
        <div className="checkmarks-container">
          {sortedCheckmarks.map((checkmark) => (
            <CheckmarkItem
              key={checkmark.id}
              checkmark={checkmark}
              onToggleComplete={toggleComplete}
              onEdit={handleEditCheckmark}
              onDelete={deleteCheckmark}
            />
          ))}
        </div>
      )}

      {sortedCheckmarks.length > 0 && (
        <div className="list-footer">
          <div className="footer-stats">
            <span>
              💡 <strong>Dica:</strong> Clique no círculo para marcar como concluído
            </span>
            <span>
              ⚡ <strong>Status:</strong> {pendingCount} pendentes, {overdueCount} atrasados
            </span>
          </div>
        </div>
      )}
    </div>
  );
};

export default CheckmarkList;