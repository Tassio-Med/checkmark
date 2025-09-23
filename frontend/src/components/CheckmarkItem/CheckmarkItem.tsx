import React from 'react';
import { Checkmark, PriorityLevel } from '../../models/Checkmark';
import './CheckmarkItem.css';

interface CheckmarkItemProps {
  checkmark: Checkmark;
  onToggleComplete: (checkmark: Checkmark) => void;
  onEdit: (checkmark: Checkmark) => void;
  onDelete: (id: number) => void;
}

const CheckmarkItem: React.FC<CheckmarkItemProps> = ({
  checkmark,
  onToggleComplete,
  onEdit,
  onDelete
}) => {
  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  };

  const getPriorityClass = (priority: PriorityLevel): string => {
    switch (priority) {
      case 2:
        return 'priority-high';
      case 1:
        return 'priority-medium';
      case 0:
        return 'priority-low';
      default:
        return '';
    }
  };

  const getPriorityIcon = (priority: PriorityLevel): string => {
    switch (priority) {
      case 2:
        return '🔴';
      case 1:
        return '🟡';
      case 0:
        return '🟢';
      default:
        return '⚪';
    }
  };

  const getPriorityText = (priority: PriorityLevel): string => {
    switch (priority) {
      case 2:
        return 'Alta';
      case 1:
        return 'Média';
      case 0:
        return 'Baixa';
      default:
        return priority.toString();
    }
  };

  const isOverdue = checkmark.dueDate && 
                   new Date(checkmark.dueDate) < new Date() && 
                   !checkmark.isCompleted;

  const getDaysUntilDue = (): number | null => {
    if (!checkmark.dueDate) return null;
    
    const dueDate = new Date(checkmark.dueDate);
    const today = new Date();
    const diffTime = dueDate.getTime() - today.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    return diffDays;
  };

  const getDueStatusText = (): string => {
    if (!checkmark.dueDate) return '';
    
    if (checkmark.isCompleted) return '✅ Concluída';
    
    const daysUntilDue = getDaysUntilDue();
    
    if (daysUntilDue === null) return '';
    
    if (daysUntilDue < 0) return `⚠️ Atrasada ${Math.abs(daysUntilDue)} dia(s)`;
    if (daysUntilDue === 0) return '⏰ Vence hoje';
    if (daysUntilDue === 1) return '📅 Vence amanhã';
    if (daysUntilDue <= 7) return `📅 Vence em ${daysUntilDue} dias`;
    
    return `📅 Vence em ${formatDate(checkmark.dueDate)}`;
  };

  const handleDelete = () => {
    if (window.confirm(`Tem certeza que deseja deletar "${checkmark.title}"?`)) {
      onDelete(checkmark.id);
    }
  };

  return (
    <div className={`checkmark-item ${checkmark.isCompleted ? 'completed' : ''} ${isOverdue ? 'overdue' : ''}`}>
      <div className="checkmark-header">
        <div className="checkmark-main-info">
          <button
            onClick={() => onToggleComplete(checkmark)}
            className={`complete-toggle ${checkmark.isCompleted ? 'completed' : ''}`}
            title={checkmark.isCompleted ? 'Marcar como pendente' : 'Marcar como concluído'}
            aria-label={checkmark.isCompleted ? 'Desmarcar como concluído' : 'Marcar como concluído'}
          >
            {checkmark.isCompleted ? '✓' : '○'}
          </button>

          <div className="title-section">
            <h3 className={checkmark.isCompleted ? 'completed' : ''}>
              {checkmark.title}
            </h3>
            <span className={`priority-badge ${getPriorityClass(checkmark.priority)}`}>
              {getPriorityIcon(checkmark.priority)} {getPriorityText(checkmark.priority)}
            </span>
          </div>
        </div>

        <div className="status-section">
          <span className={`status ${checkmark.isCompleted ? 'completed' : 'pending'}`}>
            {checkmark.isCompleted ? '✅ Concluído' : '⏳ Pendente'}
          </span>
        </div>
      </div>

      {checkmark.description && (
        <div className="checkmark-description">
          <p>{checkmark.description}</p>
        </div>
      )}

      <div className="checkmark-details">
        <div className="metadata">
          <div className="metadata-item">
            <span className="metadata-label">📋 Criado:</span>
            <span className="metadata-value">{formatDate(checkmark.createdAt)}</span>
          </div>

          {checkmark.updatedAt && (
            <div className="metadata-item">
              <span className="metadata-label">✏️ Atualizado:</span>
              <span className="metadata-value">{formatDate(checkmark.updatedAt)}</span>
            </div>
          )}

          {checkmark.dueDate && (
            <div className="metadata-item">
              <span className="metadata-label">📅 Vencimento:</span>
              <span className={`metadata-value ${isOverdue ? 'overdue' : ''}`}>
                {formatDate(checkmark.dueDate)}
                {isOverdue && <span className="overdue-indicator"> ⚠️ Atrasado</span>}
              </span>
            </div>
          )}

          {checkmark.dueDate && !checkmark.isCompleted && (
            <div className="metadata-item">
              <span className="metadata-label">⏰ Status:</span>
              <span className={`metadata-value ${isOverdue ? 'overdue' : ''}`}>
                {getDueStatusText()}
              </span>
            </div>
          )}
        </div>

        <div className="checkmark-actions">
          <button
            onClick={() => onEdit(checkmark)}
            className="action-btn edit-btn"
            title="Editar checkmark"
            aria-label="Editar checkmark"
          >
            <span className="btn-icon">✏️</span>
            <span className="btn-text">Editar</span>
          </button>

          <button
            onClick={handleDelete}
            className="action-btn delete-btn"
            title="Deletar checkmark"
            aria-label="Deletar checkmark"
          >
            <span className="btn-icon">🗑️</span>
            <span className="btn-text">Deletar</span>
          </button>
        </div>
      </div>

      <div className={`status-indicator ${checkmark.isCompleted ? 'completed' : 'pending'} ${isOverdue ? 'overdue' : ''}`}></div>
    </div>
  );
};

export default CheckmarkItem;