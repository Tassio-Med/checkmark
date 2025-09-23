import React, { useState, useEffect } from 'react';
import { Checkmark, CreateCheckmarkRequest, UpdateCheckmarkRequest, PriorityLevel } from '../../models/Checkmark';
import { checkmarkApi } from '../../services/api';
import './CheckmarkForm.css';

interface CheckmarkFormProps {
  checkmark?: Checkmark;
  onSave: () => void;
  onCancel: () => void;
}

const CheckmarkForm: React.FC<CheckmarkFormProps> = ({ checkmark, onSave, onCancel }) => {
  const initialFormData: CreateCheckmarkRequest = {
    title: '',
    description: '',
    isCompleted: false,
    priority: PriorityLevel.Medium,
    dueDate: ''
  };

  const [formData, setFormData] = useState<CreateCheckmarkRequest>(initialFormData);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [touched, setTouched] = useState<{ [key: string]: boolean }>({});

  useEffect(() => {
    if (checkmark) {
      setFormData({
        title: checkmark.title,
        description: checkmark.description || '',
        isCompleted: checkmark.isCompleted,
        priority: checkmark.priority,
        dueDate: checkmark.dueDate || ''
      });
    } else {
      setFormData(initialFormData);
    }
    setTouched({});
    setError(null);
  }, [checkmark]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    
    let processedValue: any = value;
    
    if (name === 'priority' && type === 'select-one') {
      processedValue = parseInt(value, 10);
    }
    
    if (name === 'dueDate' && type === 'date') {
      processedValue = value || '';
    }
    
    setFormData(prev => ({
      ...prev,
      [name]: processedValue
    }));
    
    setTouched(prev => ({
      ...prev,
      [name]: true
    }));
  };

  const handleBlur = (e: React.FocusEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name } = e.target;
    setTouched(prev => ({
      ...prev,
      [name]: true
    }));
  };

  const validateForm = (): boolean => {
    const errors: string[] = [];

    if (!formData.title.trim()) {
      errors.push('O título é obrigatório');
    }

    if (formData.title.length > 200) {
      errors.push('O título deve ter no máximo 200 caracteres');
    }

    if (formData.description.length > 1000) {
      errors.push('A descrição deve ter no máximo 1000 caracteres');
    }

    if (formData.dueDate) {
      const dueDate = new Date(formData.dueDate);
      const today = new Date();
      today.setHours(0, 0, 0, 0);

      if (dueDate < today) {
        errors.push('A data de vencimento não pode ser no passado');
      }
    }

    if (errors.length > 0) {
      setError(errors.join('. '));
      return false;
    }

    setError(null);
    return true;
  };

  const getFieldError = (fieldName: string): string | null => {
    if (!touched[fieldName]) return null;

    switch (fieldName) {
      case 'title':
        if (!formData.title.trim()) return 'O título é obrigatório';
        if (formData.title.length > 200) return 'Máximo 200 caracteres';
        break;
      case 'description':
        if (formData.description.length > 1000) return 'Máximo 1000 caracteres';
        break;
      case 'dueDate':
        if (formData.dueDate && new Date(formData.dueDate) < new Date()) {
          return 'Data não pode ser no passado';
        }
        break;
    }
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    setTouched({
      title: true,
      description: true,
      priority: true,
      dueDate: true
    });

    if (!validateForm()) return;

    try {
      setLoading(true);
      setError(null);

      const preparedData = {
        title: formData.title.trim(),
        description: formData.description.trim(),
        isCompleted: formData.isCompleted,
        priority: Number(formData.priority),
        dueDate: formData.dueDate || undefined
      };

      if (checkmark) {
        const updateData: UpdateCheckmarkRequest = {
          ...preparedData,
          id: checkmark.id
        };
        await checkmarkApi.update(checkmark.id, updateData);
      } else {
        await checkmarkApi.create(preparedData);
      }

      onSave();
    } catch (err: any) {
      console.error('Erro detalhado:', err);
      console.error('Resposta do servidor:', err.response?.data);
      setError('Erro ao salvar o checkmark. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const isFormValid = () => {
    return formData.title.trim() && 
           formData.title.length <= 200 && 
           formData.description.length <= 1000 &&
           (!formData.dueDate || new Date(formData.dueDate) >= new Date());
  };

  const getCharacterCount = (text: string, maxLength: number) => {
    return `${text.length}/${maxLength}`;
  };

  const getPriorityText = (priority: PriorityLevel): string => {
    switch (priority) {
      case PriorityLevel.High: return 'Alta';
      case PriorityLevel.Medium: return 'Média';
      case PriorityLevel.Low: return 'Baixa';
      default: return 'Média';
    }
  };

  const getPriorityEmoji = (priority: PriorityLevel): string => {
    switch (priority) {
      case PriorityLevel.High: return '🔴';
      case PriorityLevel.Medium: return '🟡';
      case PriorityLevel.Low: return '🟢';
      default: return '🟡';
    }
  };

  return (
    <form onSubmit={handleSubmit} className="checkmark-form">
      <div className="form-header">
        <h3>{checkmark ? 'Editar Checkmark' : 'Criar Novo Checkmark'}</h3>
        {checkmark && (
          <span className="edit-badge">Editando: #{checkmark.id}</span>
        )}
      </div>

      {error && (
        <div className="form-error">
          <span className="error-icon">⚠️</span>
          <span>{error}</span>
        </div>
      )}

      <div className="form-grid">
        <div className="form-group">
          <label htmlFor="title" className="required">Título</label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleInputChange}
            onBlur={handleBlur}
            placeholder="Digite um título descritivo..."
            className={getFieldError('title') ? 'error' : ''}
            disabled={loading}
          />
          <div className="field-info">
            {touched.title && getFieldError('title') && (
              <span className="field-error">{getFieldError('title')}</span>
            )}
            <span className="char-count">{getCharacterCount(formData.title, 200)}</span>
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="description">Descrição</label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            onBlur={handleBlur}
            placeholder="Digite uma descrição detalhada (opcional)..."
            rows={4}
            className={getFieldError('description') ? 'error' : ''}
            disabled={loading}
          />
          <div className="field-info">
            {touched.description && getFieldError('description') && (
              <span className="field-error">{getFieldError('description')}</span>
            )}
            <span className="char-count">{getCharacterCount(formData.description, 1000)}</span>
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="priority">Prioridade</label>
          <select
            id="priority"
            name="priority"
            value={formData.priority}
            onChange={handleInputChange}
            onBlur={handleBlur}
            disabled={loading}
          >
            <option value={PriorityLevel.Low}>🟢 Baixa</option>
            <option value={PriorityLevel.Medium}>🟡 Média</option>
            <option value={PriorityLevel.High}>🔴 Alta</option>
          </select>
          <div className="priority-help">
            <span className={`priority-example ${formData.priority === PriorityLevel.Low ? 'active' : ''}`}>
              Baixa: Tarefas rotineiras
            </span>
            <span className={`priority-example ${formData.priority === PriorityLevel.Medium ? 'active' : ''}`}>
              Média: Tarefas importantes
            </span>
            <span className={`priority-example ${formData.priority === PriorityLevel.High ? 'active' : ''}`}>
              Alta: Tarefas urgentes
            </span>
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="dueDate">Data de Vencimento</label>
          <input
            type="date"
            id="dueDate"
            name="dueDate"
            value={formData.dueDate ? formData.dueDate.split('T')[0] : ''}
            onChange={handleInputChange}
            onBlur={handleBlur}
            className={getFieldError('dueDate') ? 'error' : ''}
            disabled={loading}
            min={new Date().toISOString().split('T')[0]}
          />
          <div className="field-info">
            {touched.dueDate && getFieldError('dueDate') && (
              <span className="field-error">{getFieldError('dueDate')}</span>
            )}
            {formData.dueDate && (
              <span className="date-info">
                {new Date(formData.dueDate).toLocaleDateString('pt-BR')}
              </span>
            )}
          </div>
        </div>
      </div>

      <div className="form-preview">
        <h4>Preview:</h4>
        <div className="preview-card">
          <div className="preview-header">
            <span className="preview-title">
              {formData.title || '(Sem título)'}
            </span>
            <span className={`preview-priority priority-${formData.priority}`}>
              {getPriorityEmoji(formData.priority)} {getPriorityText(formData.priority)}
            </span>
          </div>
          {formData.description && (
            <p className="preview-description">{formData.description}</p>
          )}
          {formData.dueDate && (
            <div className="preview-due-date">
              📅 Vence em: {new Date(formData.dueDate).toLocaleDateString('pt-BR')}
            </div>
          )}
        </div>
      </div>

      <div className="form-actions">
        <button
          type="button"
          onClick={onCancel}
          className="cancel-btn"
          disabled={loading}
        >
          Cancelar
        </button>
        
        <button
          type="submit"
          className="submit-btn"
          disabled={loading || !isFormValid()}
        >
          {loading ? (
            <>
              <span className="spinner"></span>
              Salvando...
            </>
          ) : checkmark ? (
            '💾 Atualizar Checkmark'
          ) : (
            '✅ Criar Checkmark'
          )}
        </button>
      </div>
    </form>
  );
};

export default CheckmarkForm;